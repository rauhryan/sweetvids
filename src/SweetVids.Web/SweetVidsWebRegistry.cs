using System;
using System.Web;
using FubuMVC.StructureMap;
using FubuMVC.UI.Tags;
using Microsoft.Practices.ServiceLocation;
using Spark;
using Spark.Web.FubuMVC;
using Spark.Web.FubuMVC.ViewCreation;
using SweetVids.Core;
using StructureMap.Configuration.DSL;

namespace SweetVids.Web
{
    public class SweetVidsWebRegistry : Registry
    {
        public SweetVidsWebRegistry()
        {
            Scan(x =>
                     {
                         x.TheCallingAssembly();
                         x.WithDefaultConventions();

                         x.AddAllTypesOf<IStartable>();

                     }
                );

            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
            For<HttpRequestBase>().Use(ctx => new HttpRequestWrapper(HttpContext.Current.Request));
            ForSingletonOf<SparkViewFactory>();
            For<IServiceLocator>().Use<StructureMapServiceLocator>();
           
            For<ISparkSettings>().Use(GetSparkSettings());
            For(typeof(ISparkViewRenderer<>)).Use(typeof(SparkViewRenderer<>));
        }

        private ISparkSettings GetSparkSettings()
        {
            return new SparkSettings()
                .AddAssembly(typeof (PartialTagFactory).Assembly)
                .AddNamespace("Spark.Web.FubuMVC")
                .AddNamespace("FubuMVC.UI")
                .AddNamespace("HtmlTags");
        }
    }
}