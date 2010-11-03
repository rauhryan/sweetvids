using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;

namespace SweetVids.Web.Behaviors.VariableOutput
{
    public class VariableOutputNode : OutputNode
    {
        private readonly IList<OutputHolder> _outputs = new List<OutputHolder>();

        public VariableOutputNode()
            : base(typeof(RenderVariableOutput))
        { }

        public void AddOutput(Func<OutputFormatDetector, bool> isMatch, OutputNode output)
        {
            _outputs.Add(new OutputHolder(isMatch, output));
        }

        protected override void configureObject(ObjectDef def)
        {
            ObjectDef currentCandidate = null;
            foreach (var pair in _outputs.Reverse())
            {
                var candidate = new ObjectDef(typeof(ConditionalOutput));
                candidate.Child(typeof(Func<OutputFormatDetector, bool>), pair.Predicate);
                candidate.Dependencies.Add(new ConfiguredDependency { Definition = pair.OutputNode.ToObjectDef(), DependencyType = typeof(IActionBehavior) });
                if (currentCandidate != null)
                {
                    candidate.Dependencies.Add(new ConfiguredDependency { Definition = currentCandidate, DependencyType = typeof(ConditionalOutput) });
                }
                currentCandidate = candidate;
            }

            def.Dependencies.Add(new ConfiguredDependency { Definition = currentCandidate, DependencyType = typeof(ConditionalOutput) });
        }

        class OutputHolder
        {
            public Func<OutputFormatDetector, bool> Predicate { get; private set; }
            public OutputNode OutputNode { get; private set; }

            public OutputHolder(Func<OutputFormatDetector, bool> predicate, OutputNode outputNode)
            {
                Predicate = predicate;
                OutputNode = outputNode;
            }
        }
    }
}