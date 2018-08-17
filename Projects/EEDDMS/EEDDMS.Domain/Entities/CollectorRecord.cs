using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    /// <summary>
    /// 采集器使用记录
    /// </summary>
    public class CollectorRecord : EntityBase
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        [Display(Name = "起始时间")]
        [Required(ErrorMessage = "请输入起始时间")]
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
        /// 采集器ID
        /// </summary>
        public Guid CollectorId { get; set; }

        /// <summary>
        /// 绑定设备ID
        /// </summary>
        public Guid? EquipmentId { get; set; }

        /// <summary>
        /// 采集器
        /// </summary>
        public virtual Collector Collector { get; set; }

        /// <summary>
        /// 绑定设备
        /// </summary>
        public virtual Equipment Equipment { get; set; }
    }
}
