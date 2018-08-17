using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ARMA2StringtableEditor
{
    class StringTableHelper
    {
        /* ----- 定义StringTable节点的类型 ----- */
        public enum NodeType
        {
            Project = 0,
            Package = 1,
            Container = 2,
            Key = 4,
            EnglishString = 8,
        }
    }

    class StringTableService
    {
        string xmlDocFileName;
        XmlDocument xmlDocumentMain;
        Dictionary<string, StringTableNode> stringTableNodeDict;
        TreeView treeView;

        public Dictionary<string, StringTableNode> StringTableNodeDictionary
        {
            get { return this.stringTableNodeDict; }
        }

        public TreeView TreeView
        {
            get { return this.treeView; }
        }

        /* ----- 载入Xml文档 ----- */
        public void Load(string xmlDocumentFileName)
        {
            this.xmlDocFileName = xmlDocumentFileName;
            this.xmlDocumentMain = new XmlDocument();
            this.xmlDocumentMain.Load(this.xmlDocFileName);
            this.stringTableNodeDict = new Dictionary<string, StringTableNode>();
            this.treeView = new TreeView();
            this.LoadXmlNode(this.xmlDocumentMain.ChildNodes, this.xmlDocumentMain.Name, this.treeView.Nodes);
        }

        public void Save(string xmlDocumentFileName)
        {
            this.xmlDocumentMain.Save(xmlDocumentFileName);
        }

        /* ----- 遍历并解析Xml节点 ----- */
        private void LoadXmlNode(XmlNodeList xmlNodeList, string parentKey,TreeNodeCollection treeNodes)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                StringTableNode stringTableNode = new StringTableNode();
                TreeNode treeNode = new TreeNode();
                if (xmlNode.NodeType == XmlNodeType.Element)
                {
                    if (xmlNode.Name == "Project")
                    {
                        stringTableNode.Key = parentKey + "|" + xmlNode.Attributes["name"].Value;
                        stringTableNode.Text = xmlNode.Attributes["name"].Value;
                        stringTableNode.Type = StringTableHelper.NodeType.Project;
                        stringTableNode.XmlNode = xmlNode;
                        this.AddInStringTableNodeDict(stringTableNode.Key, stringTableNode);
                        treeNode.Text = stringTableNode.Text;
                        treeNode.Tag = stringTableNode;
                        treeNodes.Add(treeNode);
                    }
                    if (xmlNode.Name == "Package")
                    {
                        stringTableNode.Key = parentKey + "|" + xmlNode.Attributes["name"].Value;
                        stringTableNode.Text = xmlNode.Attributes["name"].Value;
                        stringTableNode.Type = StringTableHelper.NodeType.Package;
                        stringTableNode.XmlNode = xmlNode;
                        this.AddInStringTableNodeDict(stringTableNode.Key, stringTableNode);
                        treeNode.Text = stringTableNode.Text;
                        treeNode.Tag = stringTableNode;
                        treeNodes.Add(treeNode);
                    }
                    if (xmlNode.Name == "Container")
                    {
                        stringTableNode.Key = parentKey + "|" + xmlNode.Attributes["name"].Value;
                        stringTableNode.Text = xmlNode.Attributes["name"].Value;
                        stringTableNode.Type = StringTableHelper.NodeType.Container;
                        stringTableNode.XmlNode = xmlNode;
                        this.AddInStringTableNodeDict(stringTableNode.Key, stringTableNode);
                        treeNode.Text = stringTableNode.Text;
                        treeNode.Tag = stringTableNode;
                        treeNodes.Add(treeNode);
                    }
                    if (xmlNode.Name == "Key")
                    {
                        stringTableNode.Key = parentKey + "|" + xmlNode.Attributes["ID"].Value;
                        stringTableNode.Text = xmlNode.Attributes["ID"].Value;
                        stringTableNode.Type = StringTableHelper.NodeType.Key;
                        stringTableNode.XmlNode = xmlNode;
                        this.AddInStringTableNodeDict(stringTableNode.Key, stringTableNode);
                        treeNode.Text = stringTableNode.Text;
                        treeNode.Tag = stringTableNode;
                        treeNodes.Add(treeNode);
                    }
                    if (xmlNode.Name == "English")
                    {
                        stringTableNode.Key = parentKey + "|" + xmlNode.Name;
                        stringTableNode.Text = xmlNode.Name;
                        stringTableNode.Type = StringTableHelper.NodeType.EnglishString;
                        stringTableNode.XmlNode = xmlNode;
                        this.AddInStringTableNodeDict(stringTableNode.Key, stringTableNode);
                        treeNode.Text = stringTableNode.Text;
                        treeNode.Tag = stringTableNode;
                        treeNodes.Add(treeNode);
                    }
                    if ((xmlNode.HasChildNodes))
                    {
                        this.LoadXmlNode(xmlNode.ChildNodes, stringTableNode.Key, treeNode.Nodes);
                    }
                }
            }
        }

        private void AddInStringTableNodeDict(string key, StringTableNode stringTableNode)
        {
            if (!(stringTableNodeDict.ContainsKey(key)))
            {
                try
                {
                    stringTableNodeDict.Add(key, stringTableNode);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }

    class StringTableNode
    {
        string key;
        string text;
        StringTableHelper.NodeType type;
        XmlNode xmlNode;

        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public StringTableHelper.NodeType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public XmlNode XmlNode
        {
            get { return this.xmlNode; }
            set { this.xmlNode = value; }
        }
    }
}