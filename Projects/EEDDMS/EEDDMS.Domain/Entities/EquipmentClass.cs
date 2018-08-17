using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    /// <summary>
    /// 设备分类
    /// </summary>
    public class EquipmentClass : EntityBase
    {
        public EquipmentClass()
        {
            this.Children = new HashSet<EquipmentClass>();
            this.EquipmentDetails = new HashSet<EquipmentDetail>();
        }
        
        /// <summary>
        /// 父项ID
        /// </summary>
        [Display(Name = "父项ID")]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "序号")]
        public int SN { get; set; }

        /// <summary>
        /// 设备分类名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入设备分类名称")]
        [StringLength(50, ErrorMessage = "最大不能超过50个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 父项实体
        /// </summary>
        public virtual EquipmentClass Parent { get; set; }

        /// <summary>
        /// 子项集合
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual ICollection<EquipmentClass> Children { get; set; }

        /// <summary>
        /// 设备详细信息集合
        /// </summary>
        public virtual ICollection<EquipmentDetail> EquipmentDetails { get; set; }
    }
}
