namespace CoreApi.Services
{
    using FuncLib.Functions;
    using System;
    using System.Threading.Tasks;

    public interface IFlowProblemSolver
    {
        Task<Tuple<Function, Variable, Variable>> Solve(double M, int coordFunctionsCount, double cylinderRadius, double sphereRadius, double speedAtInfinity);
    }
}
