using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Text.RegularExpressions; 

namespace XmlEditor
{
    class XmlHelper
    {

        public static XmlDocument xmlDoc = new XmlDocument();
        public static XmlSchemaSet xmlSch = new XmlSchemaSet();

        public static Dictionary<string, XmlNode> xmlDocMainNodeDict = new Dictionary<string, XmlNode>();
        public static Dictionary<string, XmlNode> xmlDocChildNodeCacheDict = new Dictionary<string, XmlNode>();
        public static Dictionary<string, XmlNode> xmlDocRefNodeCacheDict = new Dictionary<string, XmlNode>();

        public static Dictionary<string, string> refNodeKeyCompareDict = new Dictionary<string, string>();

        public static Dictionary<string, XmlSchemaElement> xmlSchElementDict = new Dictionary<string,XmlSchemaElement>();

        public static void LoadXmlDocument(string XmlDocFileName)
        {
            xmlDoc.Load(XmlDocFileName);
            LoadXmlNode();
        }

        public static void LoadXmlSchema(string XmlSchFileName)
        {
            xmlSch.ValidationEventHandler += new ValidationEventHandler(xmlSch_ValidationEventHandler);
            xmlSch.Add(null, XmlSchFileName);
            xmlSch.Compile();
            LoadSchemaElement();

            xmlDoc.Schemas = xmlSch;
            ValidationEventHandler ve = new ValidationEventHandler(xmlDoc_ValidationEventHandler);
            xmlDoc.Validate(ve);
        }

        static void xmlDoc_ValidationEventHandler(object sender, ValidationEventArgs e)
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
        }

        static void xmlSch_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void LoadXmlNode()
        {
            XmlNodeList xmlNodelist = xmlDoc.ChildNodes;

            foreach (XmlNode node in xmlNodelist)
            {
                string xmlNodKey;

                if (node.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                if (node.Attributes == null || node.Attributes.Count == 0)
                {
                    xmlNodKey = (string)node.Name;
                }
                else
                {
                    if (node.Attributes["refTypeID"] != null && node.Attributes["refTypeID"].Value != String.Empty)
                    {
                        LoadRefType(node.Attributes["refTypeId"].Value);
                    }
                    if (node.Attributes["internalRefs"] != null && node.Attributes["internalRefs"].Value != String.Empty)
                    {
                        //从XSD中取Ref的Key
                    }
                    xmlNodKey = (string)node.Attributes["name"].Value;
                    if (node.Attributes["name"] == null)
                    {
                        xmlNodKey = (string)node.Name;
                    }
                    else
                    {
                        xmlNodKey = (string)node.Attributes["name"].Value;
                    }
                }

                AddInXmlDocMainNodeDict(xmlNodKey, node);
            }
        }

        public static void LoadXmlNode(string xmlParentNodeKey)
        {
            XmlNode xmlParentNode;
            GetXmlMainNodeDictValue(xmlParentNodeKey, out xmlParentNode);

            XmlNodeList xmlNodelist = xmlParentNode.ChildNodes;

            foreach (XmlNode node in xmlNodelist)
            {
                string xmlChildKey;
                string refNodeKey;

                if (node.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                if (node.Attributes == null || node.Attributes.Count == 0)
                {
                    xmlChildKey = xmlParentNodeKey + "," + (string)node.Name;
                }
                else
                {
                    if (node.Attributes["refTypeId"] != null && node.Attributes["refTypeId"].Value != String.Empty)
                    {
                        LoadRefType(node.Attributes["refTypeId"].Value);
                    }
                    if (node.Attributes["internalRefs"] != null && node.Attributes["internalRefs"].Value != String.Empty)
                    {
                        //从XSD中取Ref的Key
                    }
                    if (node.Attributes["name"] == null)
                    {
                        xmlChildKey = xmlParentNodeKey + "," + (string)node.Name;
                    }
                    else
                    {
                        xmlChildKey = xmlParentNodeKey + "," + (string)node.Attributes["name"].Value;

                        if (node.ParentNode.Attributes["refTypeId"] != null && node.ParentNode.Attributes["refTypeId"].Value != string.Empty)
                        {
                            string schElemKey;
                            GetXmlSchElementDictKey(node.ParentNode.Attributes["refTypeId"].Value, out schElemKey);

                            foreach (KeyValuePair<string, XmlNode> keyValuePair in xmlDocRefNodeCacheDict)
                            {
                                if ((keyValuePair.Key.Contains(schElemKey)))
                                {
                                    if (keyValuePair.Value.Attributes["name"].Value == node.Attributes["name"].Value)
                                    {
                                        refNodeKey = keyValuePair.Key;
                                        AddInRefNodeKeyCompareDict(xmlChildKey, refNodeKey);
                                    }
                                }
                            }
                        }
                    }
                }

                AddInXmlDocMainNodeDict(xmlChildKey, node);
                AddInXmlDocChildNodeCacheDict(xmlChildKey, node);
            }
        }

        public static void LoadRef(string refKey)
        {
            XmlNode xmlRefNode;
            GetXmlMainNodeDictValue(refKey, out xmlRefNode);

            XmlNodeList xmlNodelist = xmlRefNode.ChildNodes;

            foreach (XmlNode node in xmlNodelist)
            {
                string xmlChildKey;

                if (node.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                if (node.Attributes == null || node.Attributes.Count == 0)
                {
                    xmlChildKey = refKey + "," + (string)node.Name;
                }
                else
                {
                    if (node.Attributes["name"] == null)
                    {
                        xmlChildKey = refKey + "," + (string)node.Name;
                    }
                    else
                    {
                        xmlChildKey = refKey + "," + (string)node.Attributes["name"].Value;
                    }
                }

                AddInXmlDocMainNodeDict(xmlChildKey, node);
                AddInXmlDocRefNodeCacheDict(xmlChildKey, node);
            }
        }


        public static void LoadSchemaElement()
        {
            foreach (XmlSchema sch in xmlSch.Schemas())
            {
                foreach (XmlSchemaElement schElem in sch.Elements.Values)
                {
                    string elemKey = schElem.Name;
                    AddInXmlSchElementDict(elemKey, schElem);
                    SearchSchemaElement(elemKey, schElem);
                }
            }
        }

        static void SearchSchemaElement(string parentElemKey, XmlSchemaElement schElem)
        {
            XmlSchemaComplexType complexType = schElem.ElementSchemaType as XmlSchemaComplexType;
            XmlSchemaSequence sequence = complexType.ContentTypeParticle as XmlSchemaSequence;
            if (sequence != null)
            {
                foreach (XmlSchemaElement childElem in sequence.Items)
                {
                    string childElemKey = parentElemKey + "," + childElem.Name;
                    AddInXmlSchElementDict(childElemKey, childElem);

                    SearchSchemaElement(childElemKey, childElem);
                }
            }
        }

        static void LoadRefType(string refTypeId)
        {
            string xmlSchElemKey = string.Empty;
            GetXmlSchElementDictKey(refTypeId, out xmlSchElemKey);

            if ((xmlDocMainNodeDict.ContainsKey(xmlSchElemKey)))
            {
                LoadRef(xmlSchElemKey);
            }

            //string[] xmlNodeKey = Regex.Split(xmlSchElemKey, ",");

            //List<string> nodeKeyList = new List<string>();
            //foreach (string str in xmlNodeKey)
            //{
            //    nodeKeyList.Add(str);
            //}
        }

        //static void LoadRefTypeCaption()
        //{

        //}

        public static void AddInXmlDocMainNodeDict(string xmlNodKey, XmlNode xmlNode)
        {
            if (!(xmlDocMainNodeDict.ContainsKey(xmlNodKey)))
            {
                try
                {
                    xmlDocMainNodeDict.Add(xmlNodKey, xmlNode);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        public static void AddInXmlDocChildNodeCacheDict(string xmlNodKey, XmlNode xmlNode)
        {
            if (!(xmlDocChildNodeCacheDict.ContainsKey(xmlNodKey)))
            {
                try
                {
                    xmlDocChildNodeCacheDict.Add(xmlNodKey, xmlNode);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        public static void AddInXmlDocRefNodeCacheDict(string xmlNodKey, XmlNode xmlNode)
        {
            if (!(xmlDocRefNodeCacheDict.ContainsKey(xmlNodKey)))
            {
                try
                {
                    xmlDocRefNodeCacheDict.Add(xmlNodKey, xmlNode);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        public static void AddInRefNodeKeyCompareDict(string xmlNodKey, string xmlRefKey)
        {
            if (!(refNodeKeyCompareDict.ContainsKey(xmlNodKey)))
            {
                try
                {
                    refNodeKeyCompareDict.Add(xmlNodKey, xmlRefKey);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        public static void AddInXmlSchElementDict(string schElemKey, XmlSchemaElement schElem)
        {
            if (!(xmlSchElementDict.ContainsKey(schElemKey)))
            {
                try
                {
                    xmlSchElementDict.Add(schElemKey, schElem);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        public static void GetXmlMainNodeDictValue(string xmlNodeKey, out XmlNode xmlNode)
        {
            xmlDocMainNodeDict.TryGetValue(xmlNodeKey, out xmlNode);
        }

        public static void GetXmlSchElementDictKey(string xmlNodeKey, out string xmlSchElemKey)
        {
            xmlSchElemKey = string.Empty;
            foreach (KeyValuePair<string, XmlSchemaElement> keyValuePair in xmlSchElementDict)
            {
                if (keyValuePair.Value.Name == xmlNodeKey)
                {
                    xmlSchElemKey = keyValuePair.Key;
                    return;
                }
            }
        }

        public static void GetRefNodeCaption(string xmlNodeKey, out string refNodeCaption)
        {
            refNodeCaption = String.Empty;
            string refNodeKey;
            refNodeKeyCompareDict.TryGetValue(xmlNodeKey, out refNodeKey);
            XmlNode refNode;
            xmlDocRefNodeCacheDict.TryGetValue(refNodeKey, out refNode);
            refNodeCaption = refNode.Attributes["caption"].Value;
        }

        public static void ChangeXmlMainNodeDictValue(string xmlNodeKey, string attName, String attValue)
        {
            XmlNode xmlNode;
            GetXmlMainNodeDictValue(xmlNodeKey, out xmlNode);
            foreach (XmlAttribute xmlAtt in xmlNode.Attributes)
            {
                if (xmlAtt.Name == attName)
                {
                    xmlAtt.Value = attValue;
                }
            }
            xmlDocMainNodeDict[xmlNodeKey] = xmlNode;
        }
    }
}
