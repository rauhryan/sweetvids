using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.View;
using Spark.Web.FubuMVC.ViewCreation;
using SweetVids.Web.Conventions;

namespace SweetVids.Web.Behaviors.VariableOutput
{
    public class VariableOutputConvention : IConfigurationAction 
    { 

        public void Configure(BehaviorGraph graph)
        {
            graph.Actions().Where(x => x.HasOutputBehavior() && x.OutputType().CanBeCastTo<IEnumerable>())
                .Each(x =>
                {
                    OutputNode output = null;

                    var modelType = x.OutputType();
                    var view = getRenderViewNode(x);
                    if (x.HasOutputBehavior())
                    {
                        output = view ?? x.OfType<OutputNode>().FirstOrDefault();
                    }
                    var json = new RenderJsonNode(modelType);

                    var variableOut = new VariableOutputNode();
                    
                    if (output != null)
                        output.ReplaceWith(variableOut);
                    else
                    {
                        x.AddToEnd(variableOut);
                    }

                    variableOut.AddOutput(a => a.RenderFormat == "json", json);
                    variableOut.AddOutput(a => a.AcceptsFormat("application/json"), json);
                    variableOut.AddOutput(a => a.AcceptsFormat("text/javascript"), json);

                    if (view != null)
                    {
                        variableOut.AddOutput(a => a.AcceptsFormat("text/html"), view);
                        variableOut.AddOutput(a => a.AcceptsFormat("*/*"), view);
                    }
             

                    graph.Observer.RecordCallStatus(x, "Adding variable output behavior");

                    
                });

            graph.Actions().Where(x => x.OutputType().CanBeCastTo<IReturnJson>())
                .Each(x =>
                          {
                              OutputNode output = null;
                              var view = getRenderViewNode(x);

                              if (x.HasOutputBehavior())
                              {
                                  output = view ?? x.OfType<OutputNode>().FirstOrDefault();
                              }

                              var modelType = x.OutputType();
                              
                              var json = new CustomRenderJsonNode(modelType);

                              var variableOut = new VariableOutputNode();

                              if (output != null)
                                  output.ReplaceWith(variableOut);
                              else
                              {
                                  x.AddToEnd(variableOut);
                              }

                              variableOut.AddOutput(a => a.RenderFormat == "json", json);
                              variableOut.AddOutput(a => a.AcceptsFormat("application/json"), json);
                              variableOut.AddOutput(a => a.AcceptsFormat("text/javascript"), json);

                              if (view != null)
                              {
                                  variableOut.AddOutput(a => a.AcceptsFormat("text/html"), view);
                                  variableOut.AddOutput(a => a.AcceptsFormat("*/*"), view);
                              }
                              

                              graph.Observer.RecordCallStatus(x, "Adding variable output behavior");
                          });
        }

        private static OutputNode getRenderViewNode(ActionCall x)
        {
            return x.OfType<OutputNode>().FirstOrDefault(y => y.BehaviorType.CanBeCastTo<SparkRenderViewBehavior>());
        }
    }
}