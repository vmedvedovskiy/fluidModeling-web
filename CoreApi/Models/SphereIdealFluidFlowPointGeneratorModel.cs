namespace CoreApi.Models
{
    using CoreApi.Services;

    public class SphereIdealFluidFlowPointGeneratorModel
    {
        public double[] Coefficients { get; set; }

        public double Step { get; set; }

        public Boundary XBounds { get; set; }

        public Boundary YBounds { get; set; }
    }
}
