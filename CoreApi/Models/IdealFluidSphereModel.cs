namespace CoreApi.Models
{
    using System.ComponentModel.DataAnnotations;

    public class IdealFluidSphereModel
    {
        [Range(0, double.MaxValue)]
        public double M { get; set; }

        [Range(1, int.MaxValue)]
        public int CoordFunctionsCount { get; set; }

        [Range(0, 1)]
        public double CylinderRadius { get; set; }

        [Range(0, 1)]
        public double SphereRadius { get; set; }

        [Range(0, double.MaxValue)]
        public double SpeedAtInfinity { get; set; }

        public IdealFluidSphereModel()
        {
            this.M = 2;
            this.CoordFunctionsCount = 3;
            this.CylinderRadius = 1;
            this.SphereRadius = 0.5;
            this.SpeedAtInfinity = 0.5;
        }
    }
}
