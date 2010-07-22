using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;

namespace SweetVids.Web.Behaviors
{
    public class RssOutputNode : OutputNode 
    {
       private readonly Type _modelType;

       public RssOutputNode(Type modelType)
           : base(typeof(RenderXmlOutput<>).MakeGenericType(modelType))
        {
            _modelType = modelType;
        }

        public Type ModelType { get { return _modelType; } }
        public override string Description { get { return "Xml"; } }
    }
    public class RenderXmlOutput<T> : BasicBehavior where T : class
    {
        private readonly IFubuRequest _request;
        private readonly IOutputWriter _writer;

        public RenderXmlOutput(IOutputWriter writer, IFubuRequest request)
            : base(PartialBehavior.Executes)
        {
            _writer = writer;
            _request = request;
        }

        protected override DoNext performInvoke()
        {
            var output = _request.Get<T>();
            var serializer = new CustomXmlSerializer();
            var builder = serializer.WriteText(output);
            _writer.Write("application/rss+xml", builder);

            return DoNext.Continue;
        }
    }

    public class CustomXmlSerializer    
    {
        public string WriteText<T>(T output)
        {
            String xmlString = null;
            var memoryStream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(T));
            var xmlTextWriter = new XmlTextWriter(memoryStream, new UTF8Encoding(false));
            xmlTextWriter.Formatting = Formatting.Indented;
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(xmlTextWriter, output, ns);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            xmlString = utf8ByteArrayToString(memoryStream.ToArray());
            return xmlString;
        }
        private static String utf8ByteArrayToString(Byte[] characters)
        {
            var encoding = new UTF8Encoding(false);
            var constructedString = encoding.GetString(characters);
            return (constructedString);
        }
    }
}

