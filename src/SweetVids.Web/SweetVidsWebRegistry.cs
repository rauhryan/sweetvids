using System.Web;
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

        }
    }
}