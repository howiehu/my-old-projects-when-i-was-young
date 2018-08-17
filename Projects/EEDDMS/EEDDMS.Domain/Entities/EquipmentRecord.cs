using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    /// <summary>
    /// 设备使用记录
    /// </summary>
    public class EquipmentRecord : EntityBase
    {
        /// <summary>
        /// 使用类型
        /// </summary>
        [Display(Name = "使用类型")]
        [Required(ErrorMessage = "请选择使用类型")]
        public int Type { get; set; }

        /// <summary>
        /// 计划使用时间
        /// </summary>
        [Display(Name = "计划使用时间")]
        [Required(ErrorMessage = "请输入计划使用时间")]
        public int PlanToUseTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        [Required(ErrorMessage = "请输入开始时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(200, ErrorMessage = "最大不能超过200个字符")]
        [DataType(DataType.MultilineText)]
        public string Memo { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public Guid EquipmentId { get; set; }

        /// <summary>
        /// 单位信息Id
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// 地域信息Id
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public virtual Equipment Equipment { get; set; }

        /// <summary>
        /// 单位信息
        /// </summary>
        public virtual Unit Unit { get; set; }

        /// <summary>
        /// 地域信息
        /// </summary>
        public virtual Location Location { get; set; }
    }
}
