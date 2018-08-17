using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEDDMS.WebSite.Models
{
    /// <summary>
    /// 分页信息类
    /// </summary>
    public class PagingInfo
    {
        //集合总项
        public int TotalItems { get; set; }
        //每页显示多少条
        public int ItemsPerPage { get; set; }
        //当前页
        public int CurrentPage { get; set; }
        //总页数
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}