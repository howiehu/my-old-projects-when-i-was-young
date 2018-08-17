using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    public abstract class EntityBase
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 逻辑删除标记：true - 已逻辑删除；false - 未逻辑删除
        /// </summary>
        [Display(Name = "逻辑删除标记")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Display(Name = "记录创建时间")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 最近修改日期
        /// </summary>
        [Display(Name = "最近修改时间")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 逻辑删除日期
        /// </summary>
        [Display(Name = "逻辑删除时间")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DeleteDate { get; set; }
    }
}
