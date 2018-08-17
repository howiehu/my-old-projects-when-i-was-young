using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    /// [综合查询模块]数据字段内容别名对象
    /// </summary>
    public class IQMDataValueAlias
    {
        public IQMDataValueAlias()
        {
            this.Value = string.Empty;
            this.Alias = string.Empty;
        }

        /// <summary>
        /// [综合查询模块]数据字段内容别名对象
        /// </summary>
        /// <param name="value">字段内容</param>
        /// <param name="alias">字段内容别名</param>
        public IQMDataValueAlias(string value,string alias)
        {
            this.Value = value;
            this.Alias = alias;
        }
        /// <summary>
        /// 字段内容（值）
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 字段内容别名
        /// </summary>
        public string Alias { get; set; }
    }
}