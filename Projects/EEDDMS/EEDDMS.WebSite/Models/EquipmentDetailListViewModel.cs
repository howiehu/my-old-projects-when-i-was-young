using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EEDDMS.Domain.Entities;

namespace EEDDMS.WebSite.Models
{
    //设备详细信息视图模型
    public class EquipmentDetailListViewModel
    {
        public IEnumerable<EquipmentDetail> EquipmentDetails { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}