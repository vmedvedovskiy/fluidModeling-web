namespace CoreApi.Services.Impl
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CoreApi.Service;

    public class SphereIdealFluidProblemSolver : ISphereIdealFluidProblemSolver
    {
        public async Task<double[]> Solve(double M, int coordFunctionsCount, double cylinderRadius, double sphereRadius, double speedAtInfinity)
        {
            return Enumerable.Range(0, 10)
                .Cast<double>()
                .ToArray<double>();
        }
    }
}
