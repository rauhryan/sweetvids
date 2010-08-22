using System;
using System.Collections.Generic;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using SweetVids.Core.Domain;
using SweetVids.Core.Persistence.Conventions;
using NHibernate.SqlCommand;

namespace SweetVids.Core.Persistence
{
    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {
        public AutoPersistenceModel Generate()
        {
            var mappings = new AutoPersistenceModel();

            mappings = AutoMap.AssemblyOf<Domain.Entity>();
            mappings.Where(GetAutoMappingFilter);
            mappings.Conventions.Setup(GetConventions());
            mappings.UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();
            mappings.Setup(GetSetup());
            mappings.OverrideAll(x => x.IgnoreProperties(z => z.PropertyType.IsSubclassOf(typeof(Enumeration))));
            
            mappings.IgnoreBase<Domain.Entity>();
            mappings.UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();

            return mappings;
        }


        private Action<AutoMappingExpressions> GetSetup()
        {
            return c =>
            {
                c.IsComponentType = type => componentTypes.Contains(type); 
                
                c.SubclassStrategy = t => SubclassStrategy.JoinedSubclass;
                c.FindIdentity = property => property.Name == "Id";
                c.GetComponentColumnPrefix = name => "";
                
            };
        }

        private List<Type> componentTypes = new List<Type>();

        private Action<IConventionFinder> GetConventions()
        {
            return c =>
            {
                c.Add<PrimaryKeyConvention>();
                c.Add<CustomManyToManyTableNameConvention>();
                c.Add<CustomReferencesConvention>();
                c.Add<CustomJoinedSubclassConvention>();
                c.Add<HasManyConvention>();
                AddValidation(c);

                // Keep these last
                c.Add<DefaultStringLengthConvention>();
                c.Add<CustomForeignKeyConvention>();
                c.Add<TableNameConvention>();
; 
            };
        }

        private void AddValidation(IConventionFinder finder)
        {
            finder.Add<RequiredAttributeConvention>();
            finder.Add<MaximumStingLengthConvention>();
        }

        private static bool GetAutoMappingFilter(Type arg)
        {
            if(arg.IsSubclassOf(typeof(Domain.Entity)))
            {
                if(arg == typeof (Alias))
                    return false;

                return true;
            }

            return false;
        }
    }

    public interface IAutoPersistenceModelGenerator
    {
        AutoPersistenceModel Generate();
    }


}