using System;
using System.Linq.Expressions;
using FubuMVC.Core.View;
using FubuMVC.UI;
using HtmlTags;

namespace SweetVids.Web
{
    public static class FubuPageExtensions
    {
        public static HtmlTag Edit<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression) where T: class 
        {

            var tag = new HtmlTag("p");
            tag.Child(page.LabelFor(expression).AddClass("form-label"));
            tag.Child(page.InputFor(expression).AddClasses("form-text", "color10"));

            return tag;
        }
    }
}