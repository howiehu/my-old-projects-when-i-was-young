using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace EEDDMS.WebSite.Helpers
{
    public class MVCHelper
    {
        /// <summary>
        /// 正则表达式日期验证
        /// </summary>
        /// <param name="str">传入的要验证的字符串</param>
        /// <returns>是否符合要求</returns>
        public static bool Regex(string str)
        {
            string constStr = @"([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))";
            return System.Text.RegularExpressions.Regex.IsMatch(str, constStr);

        }
        /// <summary>
        /// 从配置文件中读取翻页要显示的条数
        /// </summary>
        /// <returns></returns>
        public static int FromConfigQueryPageSize()
        {
            string page = ConfigurationManager.AppSettings["PageSize"];
            int configPage = 4;
            if (!string.IsNullOrEmpty(page))
            {
                int.TryParse(page, out configPage);
            }

            return configPage;
        }
    }
}