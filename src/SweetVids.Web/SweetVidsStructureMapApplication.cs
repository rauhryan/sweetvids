using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using FubuMVC.Core;

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

        public virtual FubuRegistry GetMyRegistry()
        {
            return new SweetVidsFubuRegistry(HttpContext.Current.IsDebuggingEnabled, ControllerAssembly);
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteCollection routeCollection = RouteTable.Routes;
            SweetVidsStructureMapBootstrapper.Bootstrap(routeCollection, GetMyRegistry());
        }
    }
}