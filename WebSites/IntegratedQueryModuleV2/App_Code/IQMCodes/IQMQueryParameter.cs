using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///IQMQueryParameter 的摘要说明
/// </summary>
public class IQMQueryParameter
{
    public IQMQueryParameter()
    {
        this.QueryParameters = new List<string>();
    }

    public string XmlDocName { get; set; }

    /// <summary>
    /// 查询参数列表
    /// </summary>
    public IList<string> QueryParameters { get; set; }
}