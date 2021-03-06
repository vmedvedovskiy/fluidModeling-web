﻿namespace CoreApi.Services.Impl
{
    using FuncLib.Functions;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    public class GaussIntegrationService : IIntegrationService
    {
        /// <summary>
        /// <precision, <T - nodes, CG - weights>>
        /// </summary>
        private ConcurrentDictionary<int, Tuple<double[], double[]>> coefficientsCache;

        public GaussIntegrationService()
        {
            this.coefficientsCache = new ConcurrentDictionary<int, Tuple<double[], double[]>>();
        }

        public async Task<double> Integrate(Function f, int nodesCount, TransformableVariable x, TransformableVariable y, 
            Boundary xBounds, Boundary yBounds)
        {
            this.RefreshCoefficients(nodesCount);
            Tuple<double[], double[]> coefficients;
            // TODO add concurrency checks
            this.coefficientsCache.TryGetValue(nodesCount, out coefficients);
            var weights = coefficients.Item2;
            var nodes = coefficients.Item1;

            // cache this values for linear transform of gauss coefficients and nodes
            var xDiff = xBounds.High - xBounds.Low;
            var xSum = xBounds.High + xBounds.Low;

            var yDiff = yBounds.High - yBounds.Low;
            var ySum = yBounds.High + yBounds.Low;

            double total = 0;

            for (int i = 0; i < nodesCount; ++i)
            {
                for (int j = 0; j < nodesCount; ++j)
                {
                    double transformedX = 0.5 * xSum + 0.5 * xDiff * nodes[i];
                    double transformedY = 0.5 * ySum + 0.5 * yDiff * nodes[j];
                    double currentSum = weights[i] * weights[j] 
                        * f.Value(x | x.Rule(transformedX, transformedY),
                        y | y.Rule(transformedX, transformedY));

                    Add(ref total, currentSum);
                }
            }

            return total * 0.5 * xDiff * 0.5 * yDiff;
        }

        private static double Add(ref double location1, double value)
        {
            double newCurrentValue = 0;
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (newCurrentValue == currentValue)
                    return newValue;
            }
        }

        private void RefreshCoefficients(int nodesCount)
        {
            int info;
            if (this.coefficientsCache.ContainsKey(nodesCount))
            {
                return;
            }
            else
            {
                double[] T = new double[nodesCount];
                double[] CG = new double[nodesCount];

                alglib.gqgenerategausslegendre(nodesCount, out info, out T, out CG);
                var newCoefficients = Tuple.Create<double[], double[]>(T, CG);
                // just replae value
                this.coefficientsCache.AddOrUpdate(nodesCount, newCoefficients, (key, old) => old);
            }
        }
    }
}
