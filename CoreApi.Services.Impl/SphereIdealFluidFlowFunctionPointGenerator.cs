﻿namespace CoreApi.Services.Impl
{
    using FuncLib.Functions;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    // TODO: Rename to more generic name
    public class SphereIdealFluidFlowFunctionPointGenerator : IFlowFunctionPointGenerator
    {
        private readonly ICoordinateFunctionsStore coordinateFunctions;

        public SphereIdealFluidFlowFunctionPointGenerator(ICoordinateFunctionsStore coordinateFunctions)
        {
            this.coordinateFunctions = coordinateFunctions;
        }

        public async Task<IEnumerable<Services.Point>> Generate(double step, Boundary xBounds, Boundary yBounds, 
            Function f, Variable x, Variable y)
        {
            IList<Function> functions = new List<Function>();
            //// obtained by solving equation: x(x +1)/2 = coefficients.Length 
            //var coeffCount = (int)(Math.Sqrt(8 * coefficients.Length + 1) - 1) / 2;

            //var indexes = Enumerable.Range(0, coeffCount)
            //    .SelectMany(x => Enumerable.Range(0, x + 1)
            //        .Select(t => Tuple.Create<int, int>(t, x - t)))
            //    .ToList();

            //var function = omega * indexes.Select((x, i) => coefficients[i] * this.coordinateFunctions.Get(x.Item1, x.Item2, this.x, this.y))
            //    .ToList()
            //    .Aggregate<Function>((sum, f) => sum + f);

            var xPoints = await GetEvaluationPoints(step, xBounds);
            var yPoints = await GetEvaluationPoints(step, yBounds);
            var result = new List<Services.Point>();

            for (int i = 0; i < yPoints.Count(); ++i)
            {
                List<Services.Point> inner = new List<Services.Point>();
                for (int j = 0; j < xPoints.Count(); ++j)
                {
                    inner.Add(new Services.Point()
                    {
                        X = xPoints[j],
                        Y = yPoints[i],
                        Z = f.Value(x | xPoints[j], y | yPoints[i])
                    });
                }

                result.AddRange(inner);
            }

            return result;

            return null;
        }

        private static Task<double[]> GetEvaluationPoints(double step, Boundary xBounds)
        {
            return Task.Factory.StartNew<double[]>(() =>
                {
                    var count = (long)Math.Floor((xBounds.High - xBounds.Low) / step) + 1;
                    var result = new double[count];
                    var max = result.Length;
                    for (int i = 0; i < count; i++)
                    {
                        result[i] = xBounds.Low + i * step;
                    }

                    return result;
                });
        }
    }
}
