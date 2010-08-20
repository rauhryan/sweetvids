using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuMVC.UI.Tags;
using Microsoft.Practices.ServiceLocation;
using Spark;
using Spark.Web.FubuMVC;
using Spark.Web.FubuMVC.ViewCreation;
using StructureMap;
using SweetVids.Core;

namespace SweetVids.Web
{
    public class SweetVidsStructureMapApplication : HttpApplication 
    {
        private string _controllerAssembly;
        private bool? _enableDiagnostics;

        public bool EnableDiagnostics { get { return _enableDiagnostics ?? HttpContext.Current.IsDebuggingEnabled; } set { _enableDiagnostics = value; } }

        public string ControllerAssembly { get { return _controllerAssembly ?? FindClientCodeAssembly(GetType().Assembly); } set { _controllerAssembly = value; } }

        private static string FindClientCodeAssembly(Assembly globalAssembly)
        {
            return globalAssembly
                .GetReferencedAssemblies()
                .First(name => !(name.Name.Contains("System.") && !(name.Name.Contains("mscorlib"))))
                .Name;
        }

        protected virtual SparkSettings GetSparkSettings()
        {
            return new SparkSettings()
                .AddAssembly(typeof(PartialTagFactory).Assembly)
                .AddNamespace("Spark.Web.FubuMVC")
                .AddNamespace("FubuMVC.UI")
                .AddNamespace("HtmlTags")
                .AddNamespace("System.Collections.Generic")
                .AddNamespace("System.Linq")
                .AddNamespace("SweetVids.Core")
                .AddNamespace("SweetVids.Core.Domain");
        }

        protected virtual void InitializeStructureMap(IInitializationExpression ex)
        {
            ex.ForSingletonOf<SparkViewFactory>();
            ex.For<IServiceLocator>().Use<StructureMapServiceLocator>();
            ex.For<ISparkSettings>().Use(GetSparkSettings);
            ex.For(typeof(ISparkViewRenderer<>)).Use(typeof(SparkViewRenderer<>));
            ex.AddRegistry(new SweetVidsCoreRegistry());
            ex.AddRegistry(new SweetVidsWebRegistry());
        }

        public virtual FubuRegistry GetMyRegistry()
        {
            var sparkViewFactory = ObjectFactory.Container.GetInstance<SparkViewFactory>();
            return new SweetVidsFubuRegistry(HttpContext.Current.IsDebuggingEnabled, ControllerAssembly, sparkViewFactory);
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteCollection routeCollection = RouteTable.Routes;

            ObjectFactory.Initialize(InitializeStructureMap);

            SweetVidsStructureMapBootstrapper.Bootstrap(routeCollection, GetMyRegistry());
        }
    }
}