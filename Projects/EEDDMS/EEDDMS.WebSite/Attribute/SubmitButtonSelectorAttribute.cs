using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EEDDMS.WebSite.Attribute
{
    /// <summary>
    /// 表单提交按钮选择器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SubmitButtonSelectorAttribute : ActionNameSelectorAttribute
    {
        public SubmitButtonSelectorAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 需要匹配的表单提交按钮的Name属性值
        /// </summary>
        public string Name { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, System.Reflection.MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return false;
            }

            return controllerContext.HttpContext.Request.Form.AllKeys.Contains(this.Name);
        }
    }
}