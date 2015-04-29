namespace CoreApi.Services.Impl
{
    using FuncLib.Functions;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    // TODO: Rename to more generic name
    public class FunctionPointGenerator : IFlowFunctionPointGenerator
    {
        private readonly ICoordinateFunctionsStore coordinateFunctions;

        public FunctionPointGenerator(ICoordinateFunctionsStore coordinateFunctions)
        {
            this.coordinateFunctions = coordinateFunctions;
        }

        public async Task<IEnumerable<Services.Point>> Generate(double step, Boundary xBounds, Boundary yBounds, 
            Function f, Variable x, Variable y)
        {
            var functions = new List<Function>();
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
                        X = Math.Round(xPoints[j], 3),
                        Y = Math.Round(yPoints[i], 3),
                        Z = Math.Round(f.Value(x | xPoints[j], y | yPoints[i]), 6)
                    });
                }

                result.AddRange(inner);
            }

            return result;
        }


        public async Task<IEnumerable<Services.Point>> GenerateContour(double step, double[] levels, Boundary xBounds, Boundary yBounds,
            Function f, Variable x, Variable y)
        {
            var points = await this.Generate(step, xBounds, yBounds, f, x, y);
            var xPointsCount = GetEvaluationPointsCount(step, xBounds);
            var yPointsCount = GetEvaluationPointsCount(step, yBounds);

            return null;
        }

        private static Task<double[]> GetEvaluationPoints(double step, Boundary bounds)
        {
            return Task.Factory.StartNew<double[]>(() =>
                {
                    var count = GetEvaluationPointsCount(step, bounds);
                    var result = new double[count];
                    var max = result.Length;
                    for (int i = 0; i < count; i++)
                    {
                        result[i] = bounds.Low + i * step;
                    }

                    return result;
                });
        }

        private static long GetEvaluationPointsCount(double step, Boundary bounds)
        {
            return (long)Math.Floor((bounds.High - bounds.Low) / step) + 1;
        }
    }
}
