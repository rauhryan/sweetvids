using System.Web.Routing;
using FluentNHibernate;
using SweetVids.Core;
using SweetVids.Core.Util;
using SweetVids.Web.Behaviors;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using StructureMap;

namespace SweetVids.Web
{
    public class SweetVidsStructureMapBootstrapper
    {
        private readonly RouteCollection _routes;

        private SweetVidsStructureMapBootstrapper(RouteCollection routes)
        {
            _routes = routes;
        }

        public static void Bootstrap(RouteCollection routes, FubuRegistry fubuRegistry)
        {
            new SweetVidsStructureMapBootstrapper(routes).BootstrapStructureMap(fubuRegistry);
        }

        private void BootstrapStructureMap(FubuRegistry fubuRegistry)
        {
            UrlContext.Reset();

            var fubuBootstrapper = new StructureMapBootstrapper(ObjectFactory.Container, fubuRegistry);
            //fubuBootstrapper.Builder = (c, args, id) =>
            //                               {
            //                                   return new TransactionalContainerBehavior(c, args, id);
            //                               };

            fubuBootstrapper.Bootstrap(_routes);

        

            ObjectFactory.Container.StartStartables();
            
        }
    }
}