using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    /// <summary>
    /// 采集器信息
    /// </summary>
    public class Collector : EntityBase
    {
        public Collector()
        {
            this.CollectorRecords = new HashSet<CollectorRecord>();
        }

        /// <summary>
        /// 采集器编号
        /// </summary>
        [Display(Name = "采集器编号")]
        [Required(ErrorMessage = "请输入采集器编号")]
        [StringLength(50, ErrorMessage = "最大不能超过50个字符")]
        public string CollectorNo { get; set; }

        /// <summary>
        /// 采集器状态（考虑改为一个单独的描述表）
        /// </summary>
        [Display(Name = "采集器状态")]
        [Required(ErrorMessage = "请选择采集器状态")]
        public int State { get; set; }

        /// <summary>
        /// 出厂日期
        /// </summary>
        [Display(Name = "出厂日期")]
        [Required(ErrorMessage = "请输入出厂日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 启用日期
        /// </summary>
        [Display(Name = "启用日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Editable(false)]
        public DateTime? StartToUseDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(200, ErrorMessage = "最大不能超过200个字符")]
        [DataType(DataType.MultilineText)]
        public string Memo { get; set; }

        /// <summary>
        /// 设备信息ID
        /// </summary>
        public Guid? EquipmentId { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public virtual Equipment Equipment { get; set; }

        /// <summary>
        /// 采集器使用记录集合
        /// </summary>
        public virtual ICollection<CollectorRecord> CollectorRecords { get; set; }
    }
}
