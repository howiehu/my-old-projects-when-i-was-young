using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    /// <summary>
    /// 设备制造商
    /// </summary>
    public class Manufacturer : EntityBase
    {
        public Manufacturer()
        {
            this.EquipmentDetails = new HashSet<EquipmentDetail>();
        }

        /// <summary>
        /// 设备制造商名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入设备制造商名称")]
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
        /// 设备详细信息集合
        /// </summary>
        public virtual ICollection<EquipmentDetail> EquipmentDetails { get; set; }
    }
}
