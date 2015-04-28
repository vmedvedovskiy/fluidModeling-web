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

        public async Task<Tuple<Function, Variable, Variable>> Solve(double M, int n, double cylinderRadius,
            double sphereRadius, double speedAtInfinity)
        {

            var coordFunctions = this.coordinateFunctions.Get(n);
            var coordFunctionsCount = coordFunctions.Count;

            #region Variables

            var rr = new TransformableVariable(0);
            var zz = new TransformableVariable(1);

            var ro = new TransformableVariable(0, (a, b) =>
                Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)));

            var phi = new TransformableVariable(1, (a, b) =>
            {
                if (a < 0)
                {
                    return Math.Atan2(b, a) + Math.PI;
                }

                return Math.Atan2(b, a);
            });

            #endregion

            #region Variable boundaries

            var xBounds = new Boundary() { Low = -M, High = M };
            var yBounds = new Boundary() { Low = 0, High = cylinderRadius };

            var roBounds = new Boundary() { Low = 0, High = sphereRadius };
            var phiBounds = new Boundary() { Low = 0, High = Math.PI };

            #endregion

            #region Functions definition

            Func<Function, Variable, Variable, Function> A =
                    (Function f, Variable r, Variable z) => (-1 / r) *
                    f.Derivative(z, 2) - (((1 / r) * f.Derivative(r)).Derivative(r));

            Func<Variable, Variable, Function> omega1 =
                (Variable r, Variable z) => Function.Pow(r, 2)
                    * (Function.Pow(z, 2) + Function.Pow(r, 2)
                        - Math.Pow(sphereRadius, 2));

            Func<Variable, Variable, Function> omega2 =
                (Variable r, Variable z) => 1 - Function.Pow(r, 2);

            Func<Variable, Variable, Function> omega =
                (Variable r, Variable z) => omega1(r, z) * omega2(r, z)
                    / Function.Pow((1 + Function.Pow(z, 2)), 2);

            Func<Variable, Variable, Function> X =
                (Variable r, Variable z) => 1 + Function.Pow(z, 2);

            // c - значение функции на Г2 - радиус трубы
            Func<Variable, Variable, double, Function> F0 =
                (Variable r, Variable z, double c) => c * omega1(r, z)
                    / (omega1(r, z) + X(r, z) * omega2(r, z));

            Func<double[], Variable, Variable, Function> result =
                (double[] coeff, Variable r, Variable z) =>
                {
                    return omega(r, z) * coeff.Select((c, i) => c * coordFunctions[i](r, z))
                        .ToList()
                        .Aggregate<Function>((sum, current) => sum + current);
                };

            #endregion

            var matrix = new double[coordFunctionsCount, coordFunctionsCount];
            var rightPart = new double[coordFunctionsCount];

            for (int i = 0; i < coordFunctionsCount; ++i)
            {
                var right = A(F0(rr, zz, cylinderRadius), rr, zz) * coordFunctions[(int)i](rr, zz);

                for (int j = 0; j < coordFunctionsCount; ++j)
                {
                    var cylinderPart = A(coordFunctions[(int)i](rr, zz), rr, zz) * coordFunctions[(int)j](rr, zz);

                    var spherePart = A(coordFunctions[(int)i](ro, phi), ro, phi) * coordFunctions[(int)j](ro, phi);

                    matrix[(int)i, (int)j] = await integrationService.Integrate(
                        cylinderPart, 50, rr, zz, xBounds, yBounds)
                         - await integrationService.Integrate(
                        spherePart, 50, ro, phi, roBounds, phiBounds);
                }

                rightPart[i] = await integrationService.Integrate(right, 10, rr, zz, xBounds, yBounds);
            }


            int info;
            double[] coefficients;
            alglib.densesolverreport report;

            alglib.rmatrixsolve(matrix, coordFunctionsCount, rightPart, out info, out report, out coefficients);

            return Tuple.Create(result(coefficients, rr, zz), rr as Variable, zz as Variable);
        }
    }
}
