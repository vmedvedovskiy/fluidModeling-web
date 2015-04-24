namespace CoreApi.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using CoreApi.Results;
    using CoreApi.Models;
    using CoreApi.Services;

    public class IdealFluidSphereController : ApiController
    {
        private readonly IFlowProblemSolver solver;
        private readonly IFlowFunctionPointGenerator pointGenerator;

        public IdealFluidSphereController(IFlowProblemSolver solver, IFlowFunctionPointGenerator pointGenerator)
        {
            this.solver = solver;
            this.pointGenerator = pointGenerator;
        }

        /// <summary>
        /// Solves problem for sphere ideal fluid flow.
        /// Returns coefficients, built using Ritz method for approximate solution
        /// </summary>
        /// <param name="M">Integration limits</param>
        /// <param name="coordFunctionsCount">Count of coordinate functions for approximation</param>
        /// <param name="cylinderRadius">Radius of the cylinder. Must be in range between 0 and 1</param>
        /// <param name="sphereRadius">Radius of the sphere. Must be in range between 0 and 1</param>
        /// <param name="speedAtInfinity">Speed of fluid flow at infinity range.</param>
        /// <returns></returns>

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetPoints([FromBody]SphereIdealFluidFlowPointGeneratorModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await this.solver.Solve(model.M, model.CoordFunctionsCount, 
                model.CylinderRadius, model.SphereRadius, model.SpeedAtInfinity);

            return new PointsActionResult(await this.pointGenerator.Generate(model.Step, model.XBounds, model.YBounds, result));
        }
    }
}
