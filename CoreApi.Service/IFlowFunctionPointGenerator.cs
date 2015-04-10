namespace CoreApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFlowFunctionPointGenerator
    {
        Task<IEnumerable<Point>> Generate(double[] coefficients, double step, Boundary xBounds, Boundary yBounds);
    }
}