using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;
using SweetVids.Core.Domain;

namespace SweetVids.Core.Persistence.Overrides
{
    public class CommentOverride : IAutoMappingOverride<VideoComment>
    {
        public void Override(AutoMapping<VideoComment> mapping)
        {
            //mapping.References(c => c.GetVideo())
            //    .Access.CamelCaseField(Prefix.Underscore);
        }
    }
}