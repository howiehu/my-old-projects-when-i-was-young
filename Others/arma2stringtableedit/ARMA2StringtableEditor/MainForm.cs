using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ARMA2StringtableEditor
{
    public partial class MainForm : Form
    {
        string xmlDocumentFileName;
        StringTableService stringTableService;
        bool valueChanged = false;
        TextBox textBox;
        DataTable dataTable;
        DataGridView dataGridView;

        public MainForm()
        {
            InitializeComponent();
        }

        /* ----- Xml文档载入 ----- */
        private void LoadXmlDocument()
        {
            OpenFileDialog dlgOpenXmlDoc = new OpenFileDialog();
            dlgOpenXmlDoc.Title = "Open Stringtable.xml File...";
            dlgOpenXmlDoc.Filter = "Stringtable File|stringtable.xml|All Files|*.*";
            if (dlgOpenXmlDoc.ShowDialog() == DialogResult.OK)
            {
                this.xmlDocumentFileName = dlgOpenXmlDoc.FileName;
                this.stringTableService = new StringTableService();
                this.stringTableService.Load(this.xmlDocumentFileName);
                this.TreeViewBanding();
                this.toolStripMenuItemSaveAs.Enabled = true;
            }
        }

        private void SaveXmlDocument()
        {
            this.stringTableService.Save(this.xmlDocumentFileName);
        }

        private void SaveAsXmlDocument()
        {
            SaveFileDialog dlgSaveAs = new SaveFileDialog();
            dlgSaveAs.Title = "Save As...";
            dlgSaveAs.Filter = "Xml Document|*.xml|All Files|*.*";
            if (dlgSaveAs.ShowDialog() == DialogResult.OK)
            {
                this.stringTableService.Save(dlgSaveAs.FileName);
            }
        }

        private void ValueChanged()
        {
            this.valueChanged = true;
            this.toolStripButtonSave.Enabled = true;
            this.toolStripMenuItemSave.Enabled = true;
        }

        /* ----- TreeView节点绑定 ----- */
        private void TreeViewBanding()
        {
            this.splitContainerMain.Panel1.Controls.Clear();
            this.stringTableService.TreeView.Dock = DockStyle.Fill;
            this.splitContainerMain.Panel1.Controls.Add(this.stringTableService.TreeView);
            this.stringTableService.TreeView.AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);
            this.stringTableService.TreeView.BeforeSelect += new TreeViewCancelEventHandler(TreeView_BeforeSelect);
            this.toolStripStatusLabel.Text = "Loaded";
        }

        /* ----- TreeView选定树节点前发生 ----- */
        void TreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode selectedNode = this.stringTableService.TreeView.SelectedNode;
            if (selectedNode != null)
            {
                StringTableNode selectedNodeTag = (StringTableNode)this.stringTableService.TreeView.SelectedNode.Tag;
                if (selectedNodeTag.Type == StringTableHelper.NodeType.EnglishString)
                {
                    if (this.textBox.Text != (string)this.textBox.Tag)
                    {
                        DialogResult dialogResult = MessageBox.Show("English string has changed, accept change?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        if (dialogResult == DialogResult.Yes)
                        {
                            selectedNodeTag.XmlNode.InnerText = this.textBox.Text;
                            this.ValueChanged();
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            selectedNodeTag.XmlNode.InnerText = (string)this.textBox.Tag;
                        }
                        else if (dialogResult == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        /* ----- TreeView选定树节点后发生 ----- */
        void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (((StringTableNode)e.Node.Tag).Type == StringTableHelper.NodeType.EnglishString)
            {
                this.splitContainerMain.Panel2.Controls.Clear();
                this.textBox = new TextBox();
                this.textBox.Dock = DockStyle.Fill;
                this.ImeMode = ImeMode.OnHalf;
                this.textBox.Multiline = true;
                string xmlNodeValue = ((StringTableNode)e.Node.Tag).XmlNode.InnerText;
                this.textBox.Text = xmlNodeValue;
                this.textBox.Tag = xmlNodeValue;
                this.splitContainerMain.Panel2.Controls.Add(this.textBox);
            }
            else
            {
                this.splitContainerMain.Panel2.Controls.Clear();

                this.dataTable = new DataTable();
                this.dataTable.Columns.Add("Attribute");
                this.dataTable.Columns.Add("Value");
                this.dataTable.Columns["Attribute"].ReadOnly = true;
                XmlNode xmlNode = ((XmlNode)((StringTableNode)e.Node.Tag).XmlNode);
                this.dataTable.Rows.Add("Prefix", xmlNode.Name);
                if (xmlNode.Attributes.Count != 0)
                {
                    foreach (XmlAttribute xmlAtt in xmlNode.Attributes)
                    {
                        this.dataTable.Rows.Add(xmlAtt.Name, xmlAtt.Value);
                    }
                }

                this.dataGridView = new DataGridView();
                this.dataGridView.Dock = DockStyle.Fill;
                this.dataGridView.DataSource = this.dataTable;
                this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                this.splitContainerMain.Panel2.Controls.Add(this.dataGridView);
            }
        }

        /* ----- 主窗体载入 ----- */
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.toolStripButtonSave.Enabled = false;
            this.toolStripMenuItemSave.Enabled = false;
            this.toolStripMenuItemSaveAs.Enabled = false;
        }

        /* ----- 工具栏打开按钮 ----- */
        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            this.LoadXmlDocument();
        }

        /* ----- 工具栏保存按钮 ----- */
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.SaveXmlDocument();
        }

        /* ----- 顶部菜单新建按钮 ----- */
        private void toolStripMenuItemNew_Click(object sender, EventArgs e)
        {

        }

        /* ----- 顶部菜单打开按钮 ----- */
        private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            this.LoadXmlDocument();
        }

        /* ----- 顶部菜单保存按钮 ----- */
        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            this.SaveXmlDocument();
        }

        /* ----- 顶部菜单另存为按钮 ----- */
        private void toolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            this.SaveAsXmlDocument();
        }

        /* ----- 顶部菜单退出按钮 ----- */
        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
