using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EEDDMS.WebSite.Helpers;
using EEDDMS.Domain.Entities;

namespace EEDDMS.WebSite.Models
{
    //地域信息视图模型
    public class LocationsListViewModel
    {
        public IEnumerable<Location> Locations { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}