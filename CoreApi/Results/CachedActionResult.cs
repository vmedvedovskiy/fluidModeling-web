namespace CoreApi.Results
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    public abstract class CachedActionResult<T> : IHttpActionResult
    {
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = this.GetContent<T>();
            result.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromDays(30),
                Public = true
            };

            return Task.FromResult<HttpResponseMessage>(result);
        }

        public abstract ObjectContent GetContent<T>();
    }
}
