
using Neo4jClient;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace graphs_demo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IGraphClient _neo4jClient;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            _neo4jClient = new GraphClient(new Uri(ConfigurationManager.ConnectionStrings["Graphene"].ConnectionString));
            Glimpse.Neo4jClient.Plugin.RegisterGraphClient(_neo4jClient);
        }
    }
}
