namespace CoreApi.Services
{
    using System.Threading.Tasks;

    public interface IFlowProblemSolver
    {
        Task<double[]> Solve(double M, int coordFunctionsCount, double cylinderRadius, double sphereRadius, double speedAtInfinity);
    }
}
