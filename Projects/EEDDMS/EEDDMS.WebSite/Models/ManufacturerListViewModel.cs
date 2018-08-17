using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EEDDMS.Domain.Entities;

namespace EEDDMS.WebSite.Models
{
    //设备制造商信息视图模型
    public class ManufacturerListViewModel
    {
        public IEnumerable<Manufacturer> Manufacturers { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}