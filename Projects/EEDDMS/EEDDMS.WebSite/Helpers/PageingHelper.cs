using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using EEDDMS.WebSite.Models;

namespace EEDDMS.WebSite.Helpers
{
    public static class PageingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
PagingInfo pagingInfo,
Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            //显示共有多少项
            TagBuilder itemPageshow = new TagBuilder("label");
            itemPageshow.InnerHtml = string.Format("共有{0}项", pagingInfo.TotalItems);
            //显示共有多少页
            TagBuilder totalPageshow = new TagBuilder("label");
            totalPageshow.InnerHtml = string.Format("共有{0}页", pagingInfo.TotalPages);
            //当前页
            TagBuilder currentPage = new TagBuilder("label");
            currentPage.InnerHtml = string.Format("当前第{0}页", pagingInfo.CurrentPage);
            //首页
            TagBuilder firstPage = new TagBuilder("a");
            if (pagingInfo.CurrentPage > 1)
            {
                firstPage.MergeAttribute("href", pageUrl(1));
            }
            firstPage.InnerHtml = "首页";
            //上一页
            TagBuilder previousPage = new TagBuilder("a");
            if (pagingInfo.CurrentPage > 1)
            {
                previousPage.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
            }
            previousPage.InnerHtml = "上一页";

            //下一页
            TagBuilder nextPage = new TagBuilder("a");
            if (pagingInfo.CurrentPage < pagingInfo.TotalPages)
            {
                nextPage.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
            }
            nextPage.InnerHtml = "下一页";

            //末页
            TagBuilder lastPage = new TagBuilder("a");
            if (pagingInfo.CurrentPage < pagingInfo.TotalPages)
            {
                lastPage.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
            }
            lastPage.InnerHtml = "末页";
            //跳转
            TagBuilder serach = new TagBuilder("input");
            serach.MergeAttribute("id", "serachPage");
            TagBuilder btnSerach = new TagBuilder("input");
            btnSerach.MergeAttribute("type", "button");
            btnSerach.MergeAttribute("value", "跳转");
            result.Append(itemPageshow);
            result.Append(totalPageshow);
            result.Append(currentPage);
            result.Append(firstPage);
            result.Append(new TagBuilder("label").InnerHtml = " |");
            result.Append(previousPage);
            result.Append(new TagBuilder("label").InnerHtml = " |");
            result.Append(nextPage);
            result.Append(new TagBuilder("label").InnerHtml = " |");
            result.Append(lastPage);
            result.Append(new TagBuilder("label").InnerHtml = " |");
            result.Append(serach);
            result.Append(btnSerach);
            return MvcHtmlString.Create(result.ToString());
        }
    }
}