using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace MCS.Common
{
    public static class XPathHelper
    {

        public static object Evaluate(string expressionPath, XmlDocument xPathDocument)
        {
            object obj = null;

            XPathExpression xPathExpression = XPathExpression.Compile(expressionPath);
            XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();

            switch (xPathExpression.ReturnType)
            {
                case XPathResultType.Number:
                case XPathResultType.String:
                    obj = xPathNavigator.Evaluate(xPathExpression);
                    break;

                case XPathResultType.NodeSet:
                    XPathNodeIterator nodes = xPathNavigator.Select(xPathExpression);
                    List<XmlNode> nodeList = new List<XmlNode>();
                    while (nodes.MoveNext())
                    {
                        if (nodes.Current is IHasXmlNode)
                        {
                            nodeList.Add(((IHasXmlNode)nodes.Current).GetNode());
                        }
                    }
                    obj = nodeList.ToArray();
                    break;

                case XPathResultType.Boolean:
                    obj = (bool)xPathNavigator.Evaluate(xPathExpression) ? true : false;
                    break;
            }

            return obj;
        }

    }
}
