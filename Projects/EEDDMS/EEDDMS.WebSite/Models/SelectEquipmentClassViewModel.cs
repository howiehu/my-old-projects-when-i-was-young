using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EEDDMS.Domain.Entities;

namespace EEDDMS.WebSite.Models
{
    public class SelectEquipmentClassViewModel
    {
        public object Object { get; set; }
        public ICollection<EquipmentClassTreeNode> EquipmentClassTreeNodes { get; set; }
    }
}