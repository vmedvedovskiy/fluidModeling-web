namespace CoreApi.Services
{
    using FuncLib.Functions;

    public interface IIntegrationService
    {
        double Integrate(Function f, int precision);
    }
}
