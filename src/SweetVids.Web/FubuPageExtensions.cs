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

        public static HtmlTag Pagination(this IFubuPage page, string url, int currentPage, int total)
        {
            var returnTag = new HtmlTag("div").AddClasses("f-right");

            if (currentPage > 0)
                returnTag.Child(new LinkTag("Newer Vids", url + "?Page=" + (currentPage - 1)));

            if(currentPage == 0 && total - 10 > 0)
                return new LinkTag("Older Vids", url + "?Page=1").WrapWith(new HtmlTag("div").AddClass("f-right"));

            if (total - (currentPage + 1 * 10) > 0)
            {
                returnTag.Child(new HtmlTag("span").Text(" | "));
                returnTag.Child(new LinkTag("Older Vids", url + "?Page=" + (currentPage + 1)));
            }

            return returnTag;
        }
    }
}