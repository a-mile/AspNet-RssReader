using System;
using System.Text;
using System.Web.Mvc;

using AspNet_RssReader_WebUI.ViewModels;

namespace AspNet_RssReader_WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfoViewModel pagingInfo,
            Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            if (pagingInfo.CurrentPage == 1)
            {
                TagBuilder a = new TagBuilder("a")
                {
                    InnerHtml = 1.ToString() + "/" + pagingInfo.TotalPages.ToString()
                };
                a.MergeAttribute("href", pageUrl(1));
                a.AddCssClass("selected");
                result.Append(a);

                if (pagingInfo.TotalPages > 1)
                {
                    a = new TagBuilder("a")
                    {
                        InnerHtml = ">"
                    };
                    a.MergeAttribute("href", pageUrl(2));
                    result.Append(a);
                }
                if (pagingInfo.TotalPages > 2)
                {
                    a = new TagBuilder("a")
                    {
                        InnerHtml = ">>"
                    };
                    a.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
                    result.Append(a);
                }
            }
            else
            {
                TagBuilder a = new TagBuilder("a")
                {
                    InnerHtml = "<<"
                };
                a.MergeAttribute("href", pageUrl(1));
                result.Append(a);

                if (pagingInfo.CurrentPage > 2)
                {
                    a = new TagBuilder("a")
                    {
                        InnerHtml = "<"
                    };
                    a.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
                    result.Append(a);
                }

                a = new TagBuilder("a")
                {
                    InnerHtml = pagingInfo.CurrentPage.ToString() + "/" + pagingInfo.TotalPages.ToString()
                };
                a.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage));
                a.AddCssClass("selected");
                result.Append(a);

                if (pagingInfo.CurrentPage < pagingInfo.TotalPages)
                {
                    a = new TagBuilder("a")
                    {
                        InnerHtml = ">>"
                    };
                    a.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage+1));


                    if (pagingInfo.CurrentPage + 1 < pagingInfo.TotalPages)
                    {
                        a.InnerHtml = ">";
                        result.Append(a);

                        a = new TagBuilder("a")
                        {
                            InnerHtml = ">>"
                        };
                        a.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
                        result.Append(a);
                    }
                    else
                    {
                        result.Append(a);
                    }
                }

                
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}