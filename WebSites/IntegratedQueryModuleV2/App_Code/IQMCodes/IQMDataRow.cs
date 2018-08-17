using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    ///IQMDataRow 的摘要说明
    /// </summary>
    public class IQMDataRow
    {
        public IQMDataRow()
        {
            this.Cells = new List<IQMDataCell>();
        }

        public List<IQMDataCell> Cells { get; set; }
    }
}