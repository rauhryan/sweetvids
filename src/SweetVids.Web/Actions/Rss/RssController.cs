using System.Linq;
using System.Xml.Serialization;
using FubuCore;
using FubuMVC.Core.Urls;
using HtmlTags;
using SweetVids.Core.Domain;
using SweetVids.Core.Persistence;
using SweetVids.Web.Actions.Videos;

namespace SweetVids.Web.Actions.Rss
{
    public class RssController
    {
        private readonly IRepository<Video> _repository;
        private readonly IUrlRegistry _urlRegistry;

        public RssController(IRepository<Video> repository, IUrlRegistry urlRegistry)
        {
            _repository = repository;
            _urlRegistry = urlRegistry;
        }

        public RssFeed Get()
        {

            var items = from v in _repository.Query()
                        .OrderByDescending(x => x.Created)
                        .Take(10)
                        .AsEnumerable()
                        select new RssItem()
                                   {
                                      Guid = new RssGuid(){Value = v.Id, IsPermaLink = true},
                                      Description = v.ToString(),
                                      Link = _urlRegistry.UrlFor(new GetVideoRequest(){Id = v.Id}).ToAbsoluteUrl(),
                                      PublishDate = v.Created.ToString("r"),
                                      Title = v.Title
                                   };

            var feed = new RssFeed()
                           {
                               Version = "2.0",
                               Xmlns = new XmlSerializerNamespaces(),
                               Channel = new RssChannel()
                                             {
                                                 Copyright = "© 2010 by NineCollective. All rights reserved.",
                                                 Description = "Just some sweet vids bro, what you mad?",
                                                 Language = "en-US",
                                                 Title = "Sweet Vids",
                                                 Link = _urlRegistry.UrlFor<RssController>(x => x.Get()).ToAbsoluteUrl(),
                                                 Items = items.ToArray()
                                             }
                           };
            return feed;
                      
        }

      
    }
}