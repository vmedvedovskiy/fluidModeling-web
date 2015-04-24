namespace CoreApi.Models
{
    using CoreApi.Services;

    public class SphereIdealFluidFlowPointGeneratorModel : IdealFluidSphereModel
    {
        public double Step { get; set; }

        public Boundary XBounds { get; set; }

        public Boundary YBounds { get; set; }
    }
}
