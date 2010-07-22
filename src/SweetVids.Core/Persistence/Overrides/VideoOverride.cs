using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;
using SweetVids.Core.Domain;

namespace SweetVids.Core.Persistence.Overrides
{
    public class VideoOverride : IAutoMappingOverride<Video>
    {
        public void Override(AutoMapping<Video> mapping)
        {
            mapping.HasMany(x => x.GetVideoComments())
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.SaveUpdate();
        }
    }
}