using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EEDDMS.Domain.Entities;

namespace EEDDMS.WebSite.Models
{
    /// <summary>
    /// 采集器信息视图模型
    /// </summary>
    public class CollectorListViewModel
    {
        public IEnumerable<Collector> Collectors { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}