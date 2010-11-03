using System;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using SweetVids.Web.Conventions;

namespace SweetVids.Web.Behaviors.VariableOutput
{
    public class CustomJsonBehavior<T> : BasicBehavior where T : class
    {
        private readonly IJsonWriter _writer;
        private readonly IFubuRequest _request;

        public CustomJsonBehavior(IJsonWriter writer, IFubuRequest request)
            : base(PartialBehavior.Executes)
        {
            _writer = writer;
            _request = request;
        }

        protected override FubuMVC.Core.DoNext performInvoke()
        {
            var output = _request.Get<T>().As<IReturnJson>().Flatten();
            _writer.Write(output);
            return DoNext.Continue;
        }
    }

    public class CustomRenderJsonNode : OutputNode
    {
         private readonly Type _modelType;

         public CustomRenderJsonNode(Type modelType)
            : base(typeof (CustomJsonBehavior<>).MakeGenericType(modelType))
        {
            _modelType = modelType;
        }

        public Type ModelType { get { return _modelType; } }
        public override string Description { get { return "Json"; } }

        public bool Equals(CustomRenderJsonNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._modelType, _modelType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(CustomRenderJsonNode)) return false;
            return Equals((CustomRenderJsonNode)obj);
        }

        public override int GetHashCode()
        {
            return (_modelType != null ? _modelType.GetHashCode() : 0);
        }
    }
}