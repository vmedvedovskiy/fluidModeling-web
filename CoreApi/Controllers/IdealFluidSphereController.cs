namespace CoreApi.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using CoreApi.Service;

    public class IdealFluidSphereController : ApiController
    {
        private readonly ISphereIdealFluidProblemSolver solver;

        public IdealFluidSphereController(ISphereIdealFluidProblemSolver solver)
        {
            this.solver = solver;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(double M = 2d, int coordFunctionsCount = 10, double cylinderRadius = 1d, 
            double sphereRadius = 0.5d, double speedAtInfinity = 0.5d)
        {
            var result = await this.solver.Solve(M, coordFunctionsCount, cylinderRadius, sphereRadius, speedAtInfinity);
            return Ok();
        }
    }
}
