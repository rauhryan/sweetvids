using System;
using System.Xml.Serialization;

namespace SweetVids.Web.Controllers.Rss
{
    [XmlRoot("rss")]
    public class RssFeed
    {
        [XmlElement("channel")]
        public RssChannel Channel;
        [XmlAttribute("version")]
        public string Version;
        [XmlNamespaceDeclarations()]
        public XmlSerializerNamespaces Xmlns;
    }

    public class RssChannel
    {
        [XmlElement("title")]
        public string Title;
        [XmlElement("link")]
        public string Link;
        [XmlElement("description")]
        public string Description;
        [XmlElement("language")]
        public string Language;
        [XmlElement("copyright")]
        public string Copyright;
        [XmlElement("image")]
        public RssImage Image;

        [XmlElement("item")]
        public RssItem[] Items;
    }

    public class RssItem
    {
        [XmlElement("title")]
        public string Title;
        [XmlElement("guid")]
        public RssGuid Guid;
        [XmlElement("pubDate")]
        public string PublishDate;
        [XmlElement("description")]
        public string Description;
        [XmlElement("link")]
        public string Link;
    }

    public class RssImage
    {
        [XmlElement("url")]
        public string Url;
        [XmlElement("title")]
        public string Title;
        [XmlElement("link")]
        public string Link;
    }

    public class RssGuid
    {
        [XmlText]
        public Guid Value;
        [XmlAttribute("isPermaLink")]
        public bool IsPermaLink;
    }
}