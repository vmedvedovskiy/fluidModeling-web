namespace CoreApi
{
    using CoreApi.Services;
    using CoreApi.Services.Impl;
    using Microsoft.Owin.Cors;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Routing;

    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            var container = new Container();

            container.RegisterWebApiRequest<IFlowProblemSolver, SphereIdealFluidFlowProblemSolver>();
            container.RegisterWebApiRequest<IFlowFunctionPointGenerator, SphereIdealFluidFlowFunctionPointGenerator>();
            container.RegisterWebApiRequest<IIntegrationService, GaussIntegrationService>();

            // Configure Web API for self-host. 
            HttpConfiguration configuration = new HttpConfiguration();

            configuration.Routes.MapHttpRoute(
                name: "GetPoints",
                routeTemplate: "api/sphere/points",
                defaults: new
                {
                    controller = "IdealFluidSphere",
                    action = "GetPoints"
                },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            configuration.Routes.MapHttpRoute(
                name: "GetCoefficients",
                routeTemplate: "api/sphere/coefficients",
                defaults: new
                {
                    controller = "IdealFluidSphere",
                    action = "GetCoefficients"
                },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var json = configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            json.SerializerSettings.DateParseHandling = DateParseHandling.None;

            container.RegisterWebApiControllers(configuration);
            configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            appBuilder.UseCors(CorsOptions.AllowAll);
            appBuilder.UseWebApi(configuration);
        }
    }
}