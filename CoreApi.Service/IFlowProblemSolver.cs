namespace CoreApi.Services
{
    using FuncLib.Functions;
    using System.Threading.Tasks;

    public interface IFlowProblemSolver
    {
        Task<Function> Solve(double M, int coordFunctionsCount, double cylinderRadius, double sphereRadius, double speedAtInfinity);
    }
}
