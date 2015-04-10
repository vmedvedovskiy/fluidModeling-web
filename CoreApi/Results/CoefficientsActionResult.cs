namespace CoreApi.Results
{
    using System.Net.Http;
    using System.Net.Http.Formatting;

    internal class CoefficientsActionResult : CachedActionResult<double[]>
    {
        private double[] coefficients;

        public CoefficientsActionResult(double[] coeeficients)
        {
            this.coefficients = coefficients;
        }

        public override ObjectContent GetContent<T>()
        {
            return new ObjectContent<double[]>(this.coefficients, new JsonMediaTypeFormatter());
        }
    }
}
