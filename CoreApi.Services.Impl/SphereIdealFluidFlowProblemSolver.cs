namespace CoreApi.Services.Impl
{
    using CoreApi.Services;
    using FuncLib.Functions;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class SphereIdealFluidFlowProblemSolver : IFlowProblemSolver
    {
        private readonly IIntegrationService integrationService;
        private readonly ICoordinateFunctionsStore coordinateFunctions;

        public SphereIdealFluidFlowProblemSolver(IIntegrationService integrationService,
            ICoordinateFunctionsStore coordinateFunctions)
        {
            this.integrationService = integrationService;
            this.coordinateFunctions = coordinateFunctions;
        }

        public async Task<Function> Solve(double M, int coordFunctionsCount, double cylinderRadius,
            double sphereRadius, double speedAtInfinity)
        {
            Variable x = new Variable();
            Variable y = new Variable();

            Func<Function, Variable, Variable, Function> A = 
                (Function f, Variable r, Variable z) =>  (- 1 / r) * 
                f.Derivative(z, 2) - ((1 / r) * f.Derivative(r)).Derivative(r);

            Func<Variable, Variable, Function> omega =
                (Variable r, Variable z) => (Function.Pow(r, 2) * (1 - Function.Pow(r, 2))
                    * (Function.Pow(z, 2) + Function.Pow(r, 2) - Math.Pow(sphereRadius, 2)))
                    / Function.Pow((1 + Function.Pow(z, 2)), 2);

            var coordFunctions = this.coordinateFunctions.Get(coordFunctionsCount, x, y);

            Func<Function, double[], Variable, Variable, Function> result =
                (Function f, double[] coeff, Variable r, Variable z) => {
                    return omega(r, z) * coeff.Select((c, i) => c * coordFunctions[i])
                        .ToList()
                        .Aggregate<Function>((sum, current) => sum + current);                    
                };

            return null;
            
        }
    }
}
