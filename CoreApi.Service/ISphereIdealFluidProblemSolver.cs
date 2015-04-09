namespace CoreApi.Service
{
    using System.Threading.Tasks;

    public interface ISphereIdealFluidProblemSolver
    {
        Task<double[]> Solve(double M, int coordFunctionsCount, double cylinderRadius, double sphereRadius, double speedAtInfinity);
    }
}
