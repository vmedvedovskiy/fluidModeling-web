namespace CoreApi
{
    using System.Web.Http;
    using CoreApi.Service;
    using CoreApi.Services.Impl;
    using Microsoft.Owin.Cors;
    using Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;

    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            var container = new Container();

            container.RegisterWebApiRequest<ISphereIdealFluidProblemSolver, SphereIdealFluidProblemSolver>();

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            container.RegisterWebApiControllers(config);
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            appBuilder.UseCors(CorsOptions.AllowAll);
            appBuilder.UseWebApi(config);
        }
    }
}