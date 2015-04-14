namespace CoreApi.Services
{
    using FuncLib.Functions;

    public interface IIntegrationService
    {
        double Integrate(Function f, int nodesCount, Variable x, Variable y,
            Boundary xBounds, Boundary yBounds);
    }
}
