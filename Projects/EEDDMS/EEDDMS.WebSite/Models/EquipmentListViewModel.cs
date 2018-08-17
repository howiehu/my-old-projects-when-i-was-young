using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EEDDMS.Domain.Entities;

namespace EEDDMS.WebSite.Models
{
    //设备信息视图模型
    public class EquipmentListViewModel
    {
        public IEnumerable<Equipment> Equipments { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}