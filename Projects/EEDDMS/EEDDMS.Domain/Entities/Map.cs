using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EEDDMS.Domain.Entities
{
    public class Map : EntityBase
    {

        /// <summary>
        ///  地图名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入地图名称")]
        [StringLength(50, ErrorMessage = "最大不能超过50个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 图片宽度
        /// </summary>
        [Display(Name = "图片宽度")]
        public int PictureWidth { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        [Display(Name = "图片高度")]
        public int PictureHeight { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }
    }
}
