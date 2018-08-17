using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    /// <summary>
    ///  地域信息
    /// </summary>
    public class Location : EntityBase
    {
        public Location()
        {
            this.EquipmentRecords = new HashSet<EquipmentRecord>();
        }

        /// <summary>
        /// 地域名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入地域名称")]
        [StringLength(50, ErrorMessage = "最大不能超过50个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(200, ErrorMessage = "最大不能超过200个字符")]
        [DataType(DataType.MultilineText)]
        public string Memo { get; set; }

        /// <summary>
        /// 地图信息集合
        /// </summary>
        public virtual ICollection<Map> Maps { get; set; }

        /// <summary>
        /// 设备使用记录集合
        /// </summary>
        public virtual ICollection<EquipmentRecord> EquipmentRecords { get; set; }
    }
}
