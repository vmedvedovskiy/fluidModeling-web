namespace CoreApi.Results
{
    using CoreApi.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Text;
    using System.Threading.Tasks;

    internal class PointsActionResult : CachedActionResult<IEnumerable<Point>>
    {
        private IEnumerable<Point> points;

        public PointsActionResult(IEnumerable<Point> points)
        {
            this.points = points;
        }

        public override ObjectContent GetContent<T>()
        {
            return new ObjectContent<IEnumerable<Point>>(this.points, new JsonMediaTypeFormatter());
        }
    }
}
