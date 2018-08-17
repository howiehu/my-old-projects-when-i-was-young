using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Infragistics.Win.UltraWinTree;

namespace XmlEditor
{
    public partial class MainForm : Form
    {
        /* ================================================== 公共变量声明部分开始 ================================================== */

        DataTable xmlNodeInfoTable = new DataTable();
        bool gridCellValueChanged = false;
        bool xmlValidationCallBack = false;
        string xmlFileName;

        /* ================================================== 公共变量声明部分结束 ================================================== */


        /* ================================================== 程序载入部分开始 ================================================== */

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            xmlNodeInfoTable.Columns.Add("属性");
            xmlNodeInfoTable.Columns.Add("内容");
            ultraGrid.DataSource = xmlNodeInfoTable;

            xmlNodeInfoTable.Columns["属性"].ReadOnly = true;
        }

        /* ================================================== 程序载入部分结束 ================================================== */


        /* ================================================== 自定义方法部分开始 ================================================== */

        private void xmlDoc_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    MessageBox.Show(e.Message, "错误");
                    break;
                case XmlSeverityType.Warning:
                    MessageBox.Show(e.Message, "警告");
                    break;
            }

            xmlValidationCallBack = true;
        }

        private void LoadTreeNode(bool isParentKey)
        {
            ultraTree.BeginUpdate();

            Dictionary<string, XmlNode> xmlDocNodeDict = new Dictionary<string, XmlNode>();

            if (isParentKey == false)
            {
                ultraTree.Nodes.Clear();
                xmlDocNodeDict = XmlHelper.xmlDocMainNodeDict;
            }
            else
            {
                ultraTree.ActiveNode.Nodes.Clear();
                xmlDocNodeDict = XmlHelper.xmlDocChildNodeCacheDict;
            }

            foreach (KeyValuePair<string, XmlNode> keyValuePair in xmlDocNodeDict)
            {
                string pairKey = (string)keyValuePair.Key;
                XmlNode pairNode = (XmlNode)keyValuePair.Value;

                UltraTreeNode newTreeNode = new UltraTreeNode();

                if (pairNode.Attributes == null || pairNode.Attributes.Count == 0)
                {
                    newTreeNode.Text = (string)pairNode.Name;
                    newTreeNode.Key = pairKey;
                }
                else
                {
                    if (pairNode.Attributes["caption"] != null && pairNode.Attributes["caption"].Value != string.Empty)
                    {
                        newTreeNode.Text = (string)pairNode.Attributes["caption"].Value;
                        newTreeNode.Key = pairKey;
                    }
                    else
                    {
                        if (pairNode.Attributes["name"] == null)
                        {
                            newTreeNode.Text = (string)pairNode.Name;
                            newTreeNode.Key = pairKey;
                        }
                        else
                        {
                            newTreeNode.Text = (string)pairNode.Attributes["name"].Value;
                            newTreeNode.Key = pairKey;

                            if (isParentKey == true)
                            {
                                if (pairNode.ParentNode.Attributes["refTypeId"] != null && pairNode.ParentNode.Attributes["refTypeId"].Value != string.Empty)
                                {
                                    string refCaption;
                                    XmlHelper.GetRefNodeCaption(pairKey, out refCaption);
                                    newTreeNode.Text = refCaption;
                                }
                            }
                        }
                    }
                }

                if (pairNode.HasChildNodes)
                {
                    newTreeNode.Override.ShowExpansionIndicator = ShowExpansionIndicator.Always;
                }

                if (isParentKey == false)
                {
                    try
                    {
                        ultraTree.Nodes.Add(newTreeNode);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        ultraTree.ActiveNode.Nodes.Add(newTreeNode);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            ultraTree.EndUpdate();
        }

        /* ---------- 打开按钮单击事件 ---------- */
        private void Open()
        {
            //打开并载入Xml文档
            OpenFileDialog dlgOpenXmlDoc = new OpenFileDialog();
            dlgOpenXmlDoc.Title = "打开XML文件";
            dlgOpenXmlDoc.Filter = "XML文件(*.xml)|*.xml|CONFIG文件(*.config)|*.config";
            if (dlgOpenXmlDoc.ShowDialog() == DialogResult.OK)
            {
                xmlFileName = (string)dlgOpenXmlDoc.FileName;
                XmlHelper.LoadXmlDocument(xmlFileName);
                this.LoadTreeNode(false);
                

                //打开并载入Xsd文档
                OpenFileDialog dlgOpenXsd = new OpenFileDialog();
                dlgOpenXsd.Title = "打开XSD文件";
                dlgOpenXsd.Filter = "XSD文件(*.xsd)|*.xsd";
                if (dlgOpenXsd.ShowDialog() == DialogResult.OK)
                {
                    XmlHelper.LoadXmlSchema(dlgOpenXsd.FileName);
                }
            }
        }

        /* ---------- 保存按钮单击事件 ---------- */
        private void Save()
        {
            //if (xmlDocMain.DocumentElement != null)
            //{
            //    xmlDocMain.Save(xmlFileName);
            //}
        }

        /* ---------- 另存为按钮单击事件 ---------- */
        private void SaveAs()
        {
            //if (xmlDocMain.DocumentElement != null)
            //{
            //    SaveFileDialog dlgSaveAs = new SaveFileDialog();
            //    dlgSaveAs.Title = "保存XML文件";
            //    dlgSaveAs.Filter = "XML文件(*.xml)|*.xml|CONFIG文件(*.config)|*.config";
            //    if (dlgSaveAs.ShowDialog() == DialogResult.OK)
            //    {
            //        xmlDocMain.Save(dlgSaveAs.FileName);
            //    }
            //}
        }

        /* ---------- 添加按钮单击事件 ---------- */
        private void Add()
        {
            //if (ultraTree.Focused)
            //{
            //    if (ultraTree.ActiveNode != null)
            //    {
            //        if (ultraTree.ActiveNode.Parent != null)
            //        {
            //            UltraTreeNode newTreeNode = new UltraTreeNode();
            //            newTreeNode.Key = ultraTree.ActiveNode.Parent.Key;
            //        }
            //    }
            //}
            //if (ultraGrid.Focused)
            //{
            //    MessageBox.Show("gaga");
            //}
        }

        /* ---------- 删除按钮单击事件 ---------- */
        private void Delete()
        {

        }

        /* ================================================== 自定义方法部分结束 ================================================== */


        /* ================================================== 界面控件功能部分开始 ================================================== */

        /* ---------- 工具栏按钮功能 ---------- */
        private void ultraToolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "OPEN":
                    this.Open();
                    break;

                case "SAVE":
                    this.Save();
                    break;

                case "SAVE_AS":
                    this.SaveAs();
                    break;

                case "EXIT":
                    this.Close();
                    break;

                case "ADD":
                    this.Add();
                    break;

                case "DELETE":
                    this.Delete();
                    break;
            }
        }

        /* ---------- 树状列表中的节点被选定之后 ---------- */
        private void ultraTree_AfterSelect(object sender, SelectEventArgs e)
        {
            //清空dataGridView中现有的Rows
            xmlNodeInfoTable.Rows.Clear();
            //取出ultraTree中当前被激活Node所对应的xmlNode
            string xmlNodeKey = (string)ultraTree.ActiveNode.Key;
            if (xmlNodeKey != String.Empty)
            {
                XmlNode xmlNode;
                XmlHelper.GetXmlMainNodeDictValue(xmlNodeKey, out xmlNode);
                //在xmlNodeInfoTable的第一行添加前缀Rows
                xmlNodeInfoTable.Rows.Add("限定名", xmlNode.Name);
                //如果xmlNode拥有属性则在xmlNodeInfoTable中循环添加属性行
                if (xmlNode.Attributes.Count != 0)
                {
                    foreach (XmlAttribute xmlAtt in xmlNode.Attributes)
                    {
                        xmlNodeInfoTable.Rows.Add(xmlAtt.Name, xmlAtt.Value);
                    }
                }
            }
        }

        /* ---------- 树状列表中的节点被展开之后 ---------- */
        private void ultraTree_AfterExpand(object sender, NodeEventArgs e)
        {
            ultraTree.ActiveNode = e.TreeNode;
            if (ultraTree.ActiveNode.HasNodes == false)
            {
                string xmlNodeKey = (string)ultraTree.ActiveNode.Key;

                if (xmlNodeKey != String.Empty)
                {
                    XmlHelper.xmlDocChildNodeCacheDict.Clear();
                    XmlHelper.LoadXmlNode(xmlNodeKey);
                    this.LoadTreeNode(true);
                }
            }
        }

        /* ---------- 表格中的单元格值激活之后 ---------- */
        private void ultraGrid_AfterCellActivate(object sender, EventArgs e)
        {
            //将被选中的单元格的原始值备份至Tag
            ultraGrid.ActiveCell.Tag = ultraGrid.ActiveCell.Value;
        }

        /* ---------- 表格中的单元格离开激活状态之前 ---------- */
        private void ultraGrid_BeforeCellDeactivate(object sender, CancelEventArgs e)
        {
            //在单元格离开激活状态之前检测单元格内容与备份在Tag里面的值是否不同
            if (ultraGrid.ActiveCell.Value != ultraGrid.ActiveCell.Tag)
            {
                //将改变后的值保存进Cache，然后取出变动后的xmlNode进行验证
                string xmlNodeKey = (string)ultraTree.ActiveNode.Key;
                string attName = (string)ultraGrid.ActiveCell.Row.Cells[0].Value;
                string attValue = (string)ultraGrid.ActiveCell.Text;
                XmlHelper.ChangeXmlMainNodeDictValue(xmlNodeKey, attName, attValue);
                XmlNode xmlNode;
                XmlHelper.GetXmlMainNodeDictValue(xmlNodeKey, out xmlNode);
                ValidationEventHandler ve = new ValidationEventHandler(xmlDoc_ValidationEventHandler);
                XmlHelper.xmlDoc.Validate(ve, xmlNode);
                //根据全局变量xmlValidationCallBack的状态判断是否有验证事件返回
                if (xmlValidationCallBack == true)
                {
                    //如果有验证信息返回则取消单元格离开激活状态的操作，并且将tag中的备份数据保存回Cache进行数据重置
                    e.Cancel = true;
                    ultraGrid.ActiveCell.Value = ultraGrid.ActiveCell.Tag;
                    string attValueTag = (string)ultraGrid.ActiveCell.Tag;
                    XmlHelper.ChangeXmlMainNodeDictValue(xmlNodeKey, attName, attValueTag);
                    ultraGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                    xmlValidationCallBack = false;
                }
                else
                {
                    xmlValidationCallBack = false;
                }
            }
        }

        /* ================================================== 界面控件功能部分结束 ================================================== */
    }
}
