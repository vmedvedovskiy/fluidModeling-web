﻿namespace CoreApi.Services.Impl
{
    using FuncLib.Functions;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    public class SphereIdealFluidFlowFunctionPointGenerator : IFlowFunctionPointGenerator
    {
        private Variable x;
        private Variable y;

        public SphereIdealFluidFlowFunctionPointGenerator()
        {
            this.x = new Variable();
            this.y = new Variable();
        }

        public async Task<IEnumerable<Services.Point>> Generate(double[] coefficients, double step, Boundary xBounds, Boundary yBounds)
        {
            IList<Function> functions = new List<Function>();
            // solve x(x +1)/2 = coefficients.Length 
            var coeffCount = (int)(Math.Sqrt(8 * coefficients.Length + 1) - 1) / 2;

            var indexes = Enumerable.Range(0, coeffCount)
                .SelectMany(x => Enumerable.Range(0, x + 1)
                    .Select(t => Tuple.Create<int, int>(t, x - t)))
                .ToList();

            var function = indexes.Select((x, i) => coefficients[i] * this.GetCoordinateFunction(x.Item1, x.Item2))
                .ToList()
                .Aggregate<Function>((sum, f) => sum + f);

            var xPoints = await GetEvaluationPoints(step, xBounds);
            var yPoints = await GetEvaluationPoints(step, yBounds);

            // reserve memory for all points to avoid resizes
            IList<Services.Point> result = new List<Services.Point>(xPoints.Count() * yPoints.Count());

            Parallel.For(0, yPoints.Count(), async (i) =>
            {
                Parallel.For(0, xPoints.Count(), (j) =>
                {
                    result.Add(new Services.Point()
                    {
                        X = xPoints[j],
                        Y = yPoints[i],
                        Z = function.Value(this.x | xPoints[j], this.y | yPoints[i])
                    });
                });
            });

            return result;
        }

        private static Task<double[]> GetEvaluationPoints(double step, Boundary xBounds)
        {
            return Task.Factory.StartNew<double[]>(() =>
                {
                    var count = (long)Math.Floor(xBounds.High / step) + 1;
                    var result = new double[count];
                    Parallel.For(0, count, (i) =>
                    {
                        result[i] = xBounds.Low + i * step;
                    });

                    return result;
                });
        }

        private Function GetCoordinateFunction(int i, int j)
        {
            return Function.Pow(x, i) * Function.Pow(y, j);
        }
    }
}
