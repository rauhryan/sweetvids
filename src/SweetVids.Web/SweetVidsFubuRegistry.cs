using System;
using FubuMVC.Core;
using FubuMVC.UI;

namespace SweetVids.Web
{
    public class SweetVidsFubuRegistry : FubuRegistry
    {
        public SweetVidsFubuRegistry(bool enableDiagnostics, string controllerAssemblyName)
        {
            IncludeDiagnostics(enableDiagnostics);

            Applies.ToThisAssembly();

            this.UseDefaultHtmlConventions();

            //Setup Actions
            Actions
              .IncludeTypesNamed(x => x.EndsWith("Action"));
         
            //Setup Routes



            this.StringConversions(x =>
                                       {
                                           
                                           x.IfIsType<DateTime>().ConvertBy(d => d.ToString("g"));
                                           x.IfIsType<decimal>().ConvertBy(d => d.ToString("N2"));
                                           x.IfIsType<float>().ConvertBy(f => f.ToString("N2"));
                                           x.IfIsType<double>().ConvertBy(d => d.ToString("N2"));
                                       });

         
            Views.TryToAttach(x =>
                                  {
                                      x.by_ViewModel_and_Namespace_and_MethodName();
                                      x.by_ViewModel_and_Namespace();
                                      x.by_ViewModel();
                                  });
        }
    }
    }
