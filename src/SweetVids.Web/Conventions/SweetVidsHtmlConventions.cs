using System;
using System.Text.RegularExpressions;
using FubuCore;
using FubuMVC.UI;
using HtmlTags;
using SweetVids.Core.Domain;
using SweetVids.Core.Util;
using SweetVids.Core.Validation;

namespace SweetVids.Web.Conventions
{
    public class SweetVidsHtmlConventions : HtmlConventionRegistry
    {

            public SweetVidsHtmlConventions()
            {
                numbers();
                validationAttributes();
                editors();

                
               Editors.Always.Modify((field, tag) =>
                                          {
                                              if (!tag.HasAttr("id"))
                                                  tag.Id(field.Accessor.Name.makeId());
                                          });

                Editors.Always.Modify(tag =>
                                          {
                                              if(tag.GetType() == typeof(TextboxTag))
                                              {
                                                  tag.AddClass("form-text");
                                              }
                                          });


           

                Displays.If(x => x.Accessor.PropertyType.IsType<DateTime?>()).Modify(tag =>
                {
                    if (tag.Text().IsEmpty())
                        tag.Text("-");
                });

            }

            private void editors()
            {
              
                Editors.IfPropertyIs<bool>().BuildBy(request => new CheckboxTag(request.Value<bool>()).AddClass("form-checkbox").Attr("value", request.ElementId));
                Editors.IfPropertyIs<Guid>().BuildBy(request => new HiddenTag().Attr("value", request.StringValue()));
                Editors.If(x => x.Accessor.PropertyType.IsType<DateTime>() || x.Accessor.PropertyType.IsType<DateTime?>()).Modify(tag => tag.AddClass("datepicker"));
                
                Editors.If(x => x.Accessor.FieldName.ToLower().Contains("password"))
                    .BuildBy(build => new HtmlTag("input").Attr("type", "password").AddClasses("form-text").Id("password"));
            }

            // Setting up rules for tagging elements with jQuery validation
            // metadata
            // I think that a lot of this gets added into the core Fubu as a
            // "jQueryValidationPack"
            private void numbers()
            {
                Editors.IfPropertyIs<Int32>().Attr("max", Int32.MaxValue);
                Editors.IfPropertyIs<Int16>().Attr("max", Int16.MaxValue);
                Editors.IfPropertyIs<Int64>().Attr("max", Int64.MaxValue);
                Editors.IfPropertyTypeIs(IsIntegerBased).AddClass("digits");
                Editors.IfPropertyTypeIs(IsFloatingPoint).AddClass("number");
            }

            // Declare policies for using validation attributes
            private void validationAttributes()
            {
                
                Editors.AddClassForAttribute<ValidEmailAttribute>("email");
                Editors.AddClassForAttribute<RequiredAttribute>("required");
                Editors.ModifyForAttribute<MaximumStringLengthAttribute>((tag, att) =>
                {
                    if (att.Length < Entity.UnboundedStringLength)
                    {
                        tag.Attr("maxlength", att.Length);
                    }
                });

                Editors.ModifyForAttribute<GreaterOrEqualToZeroAttribute>(tag => tag.Attr("min", 0));
                Editors.ModifyForAttribute<GreaterThanZeroAttribute>(tag => tag.Attr("min", 1));
            }

            private static bool IsIntegerBased(Type type)
            {
                return type == typeof(int) || type == typeof(long) || type == typeof(short);
            }

            private static bool IsFloatingPoint(Type type)
            {
                return type == typeof(decimal) || type == typeof(float) || type == typeof(double);
            }
            

    }

    public static class ConventionExtensions
    {
        public static string makeId(this string name)
        {
            return Regex.Replace(name, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}