using System;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;

namespace SweetVids.Web.Behaviors.VariableOutput
{
    public class RenderVariableOutput : BasicBehavior
    {
        private readonly IFubuRequest _fubuRequest;
        private readonly ConditionalOutput _outputs;

        public RenderVariableOutput(IFubuRequest fubuRequest, ConditionalOutput outputs)
            : base(PartialBehavior.Executes)
        {
            _fubuRequest = fubuRequest;
            _outputs = outputs;
        }

        protected override DoNext performInvoke()
        {
            var detector = _fubuRequest.Get<OutputFormatDetector>();

            var behavior = _outputs.GetOutputBehavior(detector);
            if (behavior != null)
            {
                behavior.Invoke();
            }
            return DoNext.Continue;
        }
    }

    public class OutputFormatDetector
    {
        public string Accept { get; set; }
        public string RenderFormat { get; set; }

        public bool AcceptsFormat(string format)
        {
            var rawFormats = Accept.Split(',').Select(f => f.Split(';')[0].Trim());
            return rawFormats.Contains(format);
        }
    }

    public class ConditionalOutput
    {
        public ConditionalOutput(Func<OutputFormatDetector, bool> condition, IActionBehavior behavior)
        {
            Condition = condition;
            Behavior = behavior;
        }

        public ConditionalOutput Inner { get; set; }
        public Func<OutputFormatDetector, bool> Condition { get; set; }
        public IActionBehavior Behavior { get; set; }

        public IActionBehavior GetOutputBehavior(OutputFormatDetector detector)
        {
            if (Condition(detector))
            {
                return Behavior;
            }
            return Inner == null ? null : Inner.GetOutputBehavior(detector);
        }
    }

}