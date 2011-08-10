using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Net;

using SPF.Configuration;

namespace SPF.Web
{
    public class XsltTransformProccessor : ITransformProccessor<string, byte[]>
    {

        public string Transform(ConfigurationElement configElement, byte[] rawData)
        {
            var xsltProccessorSettings = (configElement as WebSourceItemProccessorElement);
            var xsltFileUrl = xsltProccessorSettings.XsltPathElement.Url;
            bool enableXsltScriptTag = xsltProccessorSettings.XsltPathElement.EnableXsltScriptTag;
            var xsltArgs = xsltProccessorSettings.WebsourceItemXsltArgs;

            string xmlDataStr = UTF8Encoding.UTF8.GetString(rawData);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlDataStr);

            XslCompiledTransform xsltTrans = new XslCompiledTransform();
            loadXsltFile(xsltFileUrl, enableXsltScriptTag, xsltTrans);

            XsltArgumentList xsltArgsList = new XsltArgumentList();

            if (xsltArgs.Count == 0)
            {
                xsltArgsList = null;
            }
            else
            {
                loadXsltArgs(xsltArgs, xsltArgsList);
            }

            using (StringWriter resultText = new StringWriter())
            {
                try
                {
                    XPathNavigator xpathNav = xDoc.CreateNavigator();
                    xsltTrans.Transform(xpathNav, xsltArgsList, resultText);
                }
                catch (ArgumentNullException argNullEx)
                {
                    throw new ArgumentNullException("Xslt Transformer argumentNull Exception:", argNullEx);
                }
                catch (XsltCompileException xsltCompileEx)
                {
                    throw new ArgumentNullException("Xslt Transformer xslt compile Exception:", xsltCompileEx);
                }
                catch (XsltException xsltEx)
                {
                    throw new XsltException("Xslt Transformer xslt Exception:", xsltEx);
                }
                return resultText.ToString();
            }
        }

        public string Transform(ConfigurationElement configElement, byte[] rawData, NameValueCollection paras)
        {
            var xsltProccessorSettings = (configElement as WebSourceItemProccessorElement);
            var xsltFileUrl = xsltProccessorSettings.XsltPathElement.Url;
            bool enableXsltScriptTag = xsltProccessorSettings.XsltPathElement.EnableXsltScriptTag;
            var xsltArgs = xsltProccessorSettings.WebsourceItemXsltArgs;

            string xmlDataStr = UTF8Encoding.UTF8.GetString(rawData);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlDataStr);

            XslCompiledTransform xsltTrans = new XslCompiledTransform();
            loadXsltFile(xsltFileUrl, enableXsltScriptTag, xsltTrans);

            XsltArgumentList xsltArgsList = new XsltArgumentList();

            if (xsltArgs.Count == 0)
            {
                xsltArgsList = null;
            }
            else
            {
                loadXsltArgs(xsltArgs, xsltArgsList,paras);
            }

            using (StringWriter resultText = new StringWriter())
            {
                try
                {
                    XPathNavigator xpathNav = xDoc.CreateNavigator();
                    xsltTrans.Transform(xpathNav, xsltArgsList, resultText);
                }
                catch (ArgumentNullException argNullEx)
                {
                    throw new ArgumentNullException("Xslt Transformer argumentNull Exception:", argNullEx);
                }
                catch (XsltCompileException xsltCompileEx)
                {
                    throw new ArgumentNullException("Xslt Transformer xslt compile Exception:", xsltCompileEx);
                }
                catch (XsltException xsltEx)
                {
                    throw new XsltException("Xslt Transformer xslt Exception:", xsltEx);

                }
                return resultText.ToString();
            }
        }

        #region

        /// <summary>
        /// Load Xslt File
        /// </summary>
        /// <param name="xsltFileUrl">xslt file url</param>
        /// <param name="enableXsltScript">the tag whether enable xslt inner script function</param>
        /// <param name="xsltTrans">XslCompiledTransform object</param>
        private void loadXsltFile(string xsltFileUrl, bool enableXsltScript, XslCompiledTransform xsltTrans)
        {
            try
            {
                xsltTrans.Load(xsltFileUrl, (enableXsltScript ? XsltSettings.TrustedXslt : XsltSettings.Default), null);
            }
            catch (ArgumentNullException argNullEx)
            {
                throw new ArgumentNullException("Xslt Transformer argumentNull Exception:", argNullEx);
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                throw new FileNotFoundException("Xslt Transformer xslt file not found Exception:", fileNotFoundEx);
            }
            catch (WebException webEx)
            {
                throw new WebException("Xslt Transformer web Exception:", webEx);
            }
            catch (UriFormatException uriFormatEx)
            {
                throw new UriFormatException("Xslt Transformer uri format Exception:", uriFormatEx);
            }
            catch (XmlException xmlEx)
            {
                throw new XmlException("Xslt Transformer uri formatxml Exception:", xmlEx);
            }
            catch (Exception commonEx)
            {
                throw new Exception("Xslt Transformer Common Exception:", commonEx);
            }
        }

        /// <summary>
        /// Load Xslt Arguments
        /// </summary>
        /// <param name="xsltArgsCollection"></param>
        /// <param name="xsltArgsList"></param>
        private void loadXsltArgs(WebSourceItemProccessorXsltArgsCollection xsltArgsCollection, XsltArgumentList xsltArgsList)
        {
            foreach (WebSourceItemProccessorXsltArgsElement xsltArgsElement in xsltArgsCollection)
            {
                xsltArgsList.AddParam(xsltArgsElement.Key, String.Empty, xsltArgsElement.Value);
            }
        }

        /// <summary>
        /// Load Xslt Arguments
        /// </summary>
        /// <param name="xsltArgsCollection"></param>
        /// <param name="xsltArgsList"></param>
        /// <param name="paras"></param>
        private void loadXsltArgs(WebSourceItemProccessorXsltArgsCollection xsltArgsCollection, XsltArgumentList xsltArgsList, NameValueCollection paras)
        {
            foreach (WebSourceItemProccessorXsltArgsElement xsltArgsElement in xsltArgsCollection)
            {
                xsltArgsList.AddParam(xsltArgsElement.Key, String.Empty, (xsltArgsElement.Value.Contains("${") ? paras[xsltArgsElement.Key] : xsltArgsElement.Value));
            }
        }

        #endregion
    }
}
