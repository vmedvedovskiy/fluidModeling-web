namespace CoreApi.Services.Impl
{
    using CoreApi.Services;
    using System.Linq;
    using System.Threading.Tasks;

    public class SphereIdealFluidFlowProblemSolver : IFlowProblemSolver
    {
        private readonly IIntegrationService integrationService;

        public SphereIdealFluidFlowProblemSolver(IIntegrationService integrationService)
        {
            this.integrationService = integrationService;
        }

        public async Task<double[]> Solve(double M, int coordFunctionsCount, double cylinderRadius, 
            double sphereRadius, double speedAtInfinity)
        {
            return Enumerable.Range(0, 10)
                .Cast<double>()
                .ToArray<double>();
        }
    }
}
