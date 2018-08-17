using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    /// <summary>
    /// 设备详细信息
    /// </summary>
    public class EquipmentDetail : EntityBase
    {
        public EquipmentDetail()
        {
            this.Equipments = new HashSet<Equipment>();
        }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入设备名称")]
        [StringLength(50, ErrorMessage = "最大不能超过50个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        [Display(Name = "型号")]
        [Required(ErrorMessage = "请输入设备型号")]
        [StringLength(50, ErrorMessage = "最大不能超过50个字符")]
        public string Type { get; set; }

        /// <summary>
        /// 设计使用寿命
        /// </summary>
        [Display(Name = "设计使用寿命")]
        public int? DesignLife { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(200, ErrorMessage = "最大不能超过200个字符")]
        [DataType(DataType.MultilineText)]
        public string Memo { get; set; }

        /// <summary>
        /// 制造商ID
        /// </summary>
        public Guid ManufacturerId { get; set; }

        /// <summary>
        /// 设备分类ID
        /// </summary>
        public Guid EquipmentClassId { get; set; }

        /// <summary>
        /// 设备制造商
        /// </summary>
        public virtual Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// 设备分类
        /// </summary>
        public virtual EquipmentClass EquipmentClass { get; set; }

        /// <summary>
        /// 设备信息集合
        /// </summary>
        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}
