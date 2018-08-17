using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class Equipment : EntityBase
    {
        public Equipment()
        {
            this.Collectors = new HashSet<Collector>();
            this.EquipmentRecords = new HashSet<EquipmentRecord>();
        }

        /// <summary>
        /// 设备编号
        /// </summary>
        [Display(Name = "设备编号")]
        [Required(ErrorMessage = "请输入设备编号")]
        [StringLength(50, ErrorMessage = "最大不能超过50个字符")]
        public string EquipmentNo { get; set; }

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
        public DateTime? StartToUseDate { get; set; }

        /// <summary>
        /// 设备状态（考虑改为一个单独的描述表）
        /// </summary>
        [Display(Name = "设备状态")]
        [Required(ErrorMessage = "请选择设备状态")]
        public int State { get; set; }

        /// <summary>
        /// 健康状况
        /// </summary>
        [Display(Name = "健康状况")]
        [StringLength(50, ErrorMessage = "最大不能超过50个字符")]
        public string Health { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(200, ErrorMessage = "最大不能超过200个字符")]
        [DataType(DataType.MultilineText)]
        public string Memo { get; set; }

        /// <summary>
        /// 设备详细信息ID
        /// </summary>
        [Required(ErrorMessage = "请选择设备状态")]
        public Guid EquipmentDetailId { get; set; }

        /// <summary>
        /// 设备详细信息
        /// </summary>
        public virtual EquipmentDetail EquipmentDetail { get; set; }

        /// <summary>
        /// 采集器信息集合
        /// </summary>
        public virtual ICollection<Collector> Collectors { get; set; }

        /// <summary>
        /// 设备使用记录集合
        /// </summary>
        public virtual ICollection<EquipmentRecord> EquipmentRecords { get; set; }
    }
}
