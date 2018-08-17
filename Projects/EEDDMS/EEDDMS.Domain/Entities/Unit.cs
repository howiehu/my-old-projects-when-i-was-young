using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    /// <summary>
    /// 单位信息
    /// </summary>
    public class Unit : EntityBase
    {
        public Unit()
        {
            this.Children = new HashSet<Unit>();
            this.EquipmentRecords = new HashSet<EquipmentRecord>();
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
        /// 单位名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入单位名称")]
        [StringLength(100, ErrorMessage = "最大不能超过100个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(200, ErrorMessage = "最大不能超过200个字符")]
        [DataType(DataType.MultilineText)]
        public string Memo { get; set; }

        /// <summary>
        /// 父项实体
        /// </summary>
        public virtual Unit Parent { get; set; }

        /// <summary>
        /// 子项集合
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual ICollection<Unit> Children { get; set; }

        /// <summary>
        /// 设备使用记录集合
        /// </summary>
        public virtual ICollection<EquipmentRecord> EquipmentRecords { get; set; }
    }
}
