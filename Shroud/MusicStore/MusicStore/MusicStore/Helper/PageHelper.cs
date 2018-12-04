using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.ViewModels;

namespace MusicStore.Helper
{
    public static class PagingHelper
    {
        //HtmlHelper扩展方法，用于分页
        public static MvcHtmlString Pagination(this HtmlHelper html, PageInfo pageInfo, Func<PageInfo, string> pageLinks)
        {
            var htmlString = pageLinks(pageInfo);

            return MvcHtmlString.Create(htmlString);
        }
    }
}