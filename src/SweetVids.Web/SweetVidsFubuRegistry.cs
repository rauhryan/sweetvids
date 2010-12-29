using System;
using System.Xml.Serialization;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.UI;
using Spark.Web.FubuMVC;
using Spark.Web.FubuMVC.Extensions;
using Spark.Web.FubuMVC.ViewLocation;
using SweetVids.Web.Behaviors;
using SweetVids.Web.Behaviors.VariableOutput;
using SweetVids.Web.Controllers;
using SweetVids.Web.Controllers.Rss;
using SweetVids.Web.Controllers.Videos;
using SweetVids.Web.Conventions;

namespace SweetVids.Web
{
    public class SweetVidsFubuRegistry : SparkFubuRegistry
    {
        public SweetVidsFubuRegistry(bool enableDiagnostics, string controllerAssemblyName, SparkViewFactory sparkViewFactory) : base(sparkViewFactory)
        {
            IncludeDiagnostics(enableDiagnostics);

            Applies.ToThisAssembly();

            this.HtmlConvention<SweetVidsHtmlConventions>();

            HomeIs<VideosController>(x => x.List(new ListVideosRequest()));
            //Setup Actions
            Actions
              .IncludeTypesNamed(x => x.EndsWith("Controller"));
         
            //Setup Routes
            Routes
                .IgnoreControllerNamespaceEntirely()
                .IgnoreClassSuffix("Controller")
                .IgnoreMethodSuffix("Get")
                .IgnoreMethodSuffix("List")
                .IgnoreMethodSuffix("Details")
                .IgnoreMethodSuffix("Post")
                .IgnoreMethodSuffix("Delete")
                .ConstrainToHttpMethod(call => call.Method.Name.Equals("List") || call.Method.Name.Equals("Details") || call.Method.Name.Equals("Get"), "GET")
                .ConstrainToHttpMethod(call => call.Method.Name.Equals("Post"), "POST")
                .ConstrainToHttpMethod(call => call.Method.Name.Equals("Delete"), "DELETE")
                .ForInputTypesOf<IRequestById>(call => call.RouteInputFor(request => request.Id));

            this.StringConversions(x =>
                                       {
                                           
                                           x.IfIsType<DateTime>().ConvertBy(d => d.ToString("g"));
                                           x.IfIsType<decimal>().ConvertBy(d => d.ToString("N2"));
                                           x.IfIsType<float>().ConvertBy(f => f.ToString("N2"));
                                           x.IfIsType<double>().ConvertBy(d => d.ToString("N2"));
                                       });


            SparkPolicies
                 .AttachViewsBy(call => call.HandlerType.Name.EndsWith("Controller"),
                                     call => call.HandlerType.Name.RemoveSuffix("Controller"), call => call.Method.Name);

            Output
                .ToJson.WhenTheOutputModelIs<AjaxResponse>();
            Output
                .To(call => new RssOutputNode(call.OutputType())).WhenTheOutputModelIs<RssFeed>();

            Policies
                .Add<VariableOutputConvention>();
        }
    }

    

   
}
