﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    /// [综合查询模块]字段内容链接对象
    /// </summary>
    public class IQMDataValueUrl
    {
        public IQMDataValueUrl(string url)
        {
            this.Url = url;
        }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 链接参数
        /// </summary>
        public List<string> QueryStringList { get; set; }
    }
}