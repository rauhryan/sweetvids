using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using FluentNHibernate;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Bootstrapping;
using FubuMVC.Core.Packaging;
using FubuMVC.StructureMap;
using FubuMVC.Core.UI.Tags;
using Microsoft.Practices.ServiceLocation;
using Spark;
using Spark.Web.FubuMVC;
using Spark.Web.FubuMVC.ViewCreation;
using StructureMap;
using SweetVids.Core;
using SweetVids.Core.Util;
using SweetVids.Web.Behaviors;

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
                .AddNamespace("FubuMVC.Core.UI")
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
            UrlContext.Reset();
            RouteCollection routeCollection = RouteTable.Routes;
            Bootstrap(routeCollection);
        }

        [SkipOverForProvenance]
        public void Bootstrap(ICollection<RouteBase> routes)
        {

            ObjectFactory.Initialize(InitializeStructureMap);

            FubuApplication.For(GetMyRegistry())
                .ContainerFacility(new SweetVidsContainerFacility(ObjectFactory.Container))
                .Bootstrap(routes);
       
            ObjectFactory.Container.GetInstance<ISessionSource>().BuildSchema();

            ObjectFactory.Container.StartStartables();
        }

    }

    public class SweetVidsContainerFacility : StructureMapContainerFacility
    {
        private readonly IContainer _container;

        public SweetVidsContainerFacility(IContainer container)
            : base(container)
        {
            _container = container;
        }

        public override FubuMVC.Core.Behaviors.IActionBehavior BuildBehavior(FubuCore.Binding.ServiceArguments arguments, Guid behaviorId)
        {
            return new TransactionalContainerBehavior(_container, arguments, behaviorId);
        }
    }
}