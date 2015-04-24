namespace CoreApi.Services
{
    using FuncLib.Functions;
    using System.Threading.Tasks;

    public interface IIntegrationService
    {
        /// <summary>
        /// Gauss - Legendre integral approximation.
        /// Works only for 2D Cartesian coordinates.
        /// </summary>
        /// <param name="f">Function to integrate</param>
        /// <param name="nodesCount">Approximation nodes count. Good values are 4...10</param>
        /// <param name="x">The x variable</param>
        /// <param name="y">The y variable</param>
        /// <param name="xBounds">Integration bounds for x</param>
        /// <param name="yBounds">Integration bounds for y</param>
        /// <returns></returns>
        Task<double> Integrate(Function f, int nodesCount, TransformableVariable x, TransformableVariable y,
            Boundary xBounds, Boundary yBounds);
    }
}
