namespace CoreApi.Services
{
    using FuncLib.Functions;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFlowFunctionPointGenerator
    {
        Task<IEnumerable<Point>> Generate(double step, Boundary xBounds, Boundary yBounds, 
            Function f, Variable x, Variable y);

        Task<IEnumerable<Point>> GenerateContour(double step, double[] levels, Boundary xBounds, Boundary yBounds,
            Function f, Variable x, Variable y);
    }
}