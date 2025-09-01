using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace MCS.Framework.Localization
{
    public static class WebUtils
    {
        /// <summary>
        /// Returns a local resource from the resource set of the current active request
        ///             local resource.
        /// 
        /// </summary>
        /// <param name="resourceId">The resourceId of the item in the local resourceSet file to retrieve</param>
        /// <returns/>
        public static string LRes(string resourceId)
        {
            return HttpContext.GetLocalResourceObject(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, resourceId) as string ?? resourceId;
        }

        /// <summary>
        /// Returns a local resource for the given resource set that you specify explicitly.
        /// 
        ///             Use this method only if you need to retrieve resources from a local resource not
        ///             specific to the current request.
        /// 
        /// </summary>
        /// <param name="resourceSet">The resourceset specified as: subdir/page.aspx or page.aspx or as a virtual path (~/subdir/page.aspx)</param><param name="resourceKey">The resource ID to retrieve from the resourceset</param>
        /// <returns/>
        public static string LRes(string resourceSet, string resourceKey)
        {
            if (!resourceSet.StartsWith("~/"))
                resourceSet = "~/" + resourceSet;
            return HttpContext.GetLocalResourceObject(resourceSet, resourceKey) as string ?? resourceKey;
        }
        /// <summary>
        /// Matchevaluated to unescape string encoded Unicode character in the format of \u03AF
        /// 
        /// </summary>
        /// <param name="match"/>
        /// <returns/>
        private static string UnicodeEscapeMatchEvaluator(System.Text.RegularExpressions.Match match)
        {
            return ((char)ushort.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString();
        }
        /// <summary>
        /// Parses a JSON string into a string value
        /// 
        /// </summary>
        /// <param name="encodedString">JSON string</param>
        /// <returns>
        /// unencoded string
        /// </returns>
        public static string DecodeJsString(string encodedString)
        {
            if (encodedString == null)
                return (string)null;
            if (encodedString == "null")
                return (string)null;
            if (!encodedString.StartsWith("\"") || !encodedString.EndsWith("\""))
                encodedString = "\"" + encodedString + "\"";
            if (encodedString == "\"\"")
                return string.Empty;
            encodedString = encodedString.Substring(1, encodedString.Length - 2);
            encodedString = encodedString.Replace("\\\\", "^#^#");
            encodedString = encodedString.Replace("\\r", "\r");
            encodedString = encodedString.Replace("\\n", "\n");
            encodedString = encodedString.Replace("\\\"", "\"");
            encodedString = encodedString.Replace("\\t", "\t");
            encodedString = encodedString.Replace("\\b", "\b");
            encodedString = encodedString.Replace("\\f", "\f");
            if (encodedString.Contains("\\u"))
                encodedString = Regex.Replace(encodedString, "\\\\u....", new MatchEvaluator(WebUtils.UnicodeEscapeMatchEvaluator));
            encodedString = encodedString.Replace("^#^#", "\\");
            return encodedString;
        }

        /// <summary>
        /// Returns a site relative HTTP path from a partial path starting out with a ~.
        ///             Same syntax that ASP.Net internally supports but this method can be used
        ///             outside of the Page framework.
        /// 
        ///             Works like Control.ResolveUrl including support for ~ syntax
        ///             but returns an absolute URL.
        /// 
        /// </summary>
        /// <param name="originalUrl">Any Url including those starting with ~ for virtual base path replacement</param>
        /// <returns>
        /// relative url
        /// </returns>
        /// 
        /// <remarks>
        /// Returns the path as relative of current location (ie. ./link.htm) if
        ///             HttpContext is not available. Note that this may result in some scenarios where
        ///             an invalid URL is returned if HttpContext is not present, but it allows for test
        ///             scenarios.
        /// 
        /// </remarks>
        public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
                return (string)null;
            if (!originalUrl.StartsWith("~"))
                return originalUrl;
            return (HttpContext.Current == null ? "./" + originalUrl.Substring(1) : HttpContext.Current.Request.ApplicationPath + originalUrl.Substring(1)).Replace("//", "/");
        }
        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        ///             is essentially a JSON string that is returned in double quotes.
        /// 
        ///             The string returned includes outer quotes:
        ///             "Hello \"Rick\"!\r\nRock on"
        /// 
        /// </summary>
        /// <param name="s"/>
        /// <returns/>
        public static string EncodeJsString(string s)
        {
            if (s == null)
                return "null";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("\"");
            foreach (char ch in s)
            {
                switch (ch)
                {
                    case '\b':
                        stringBuilder.Append("\\b");
                        break;
                    case '\t':
                        stringBuilder.Append("\\t");
                        break;
                    case '\n':
                        stringBuilder.Append("\\n");
                        break;
                    case '\f':
                        stringBuilder.Append("\\f");
                        break;
                    case '\r':
                        stringBuilder.Append("\\r");
                        break;
                    case '"':
                        stringBuilder.Append("\\\"");
                        break;
                    case '\\':
                        stringBuilder.Append("\\\\");
                        break;
                    default:
                        int num = (int)ch;
                        if (num < 32 || num > (int)sbyte.MaxValue)
                        {
                            stringBuilder.AppendFormat("\\u{0:X04}", (object)num);
                            break;
                        }
                        else
                        {
                            stringBuilder.Append(ch);
                            break;
                        }
                }
            }
            stringBuilder.Append("\"");
            return ((object)stringBuilder).ToString();
        }

        /// <summary>
        /// Determines if GZip is supported
        /// 
        /// </summary>
        /// 
        /// <returns/>
        public static bool IsGZipSupported()
        {
            string str = HttpContext.Current.Request.Headers["Accept-Encoding"];
            return !string.IsNullOrEmpty(str) && (str.Contains("gzip") || str.Contains("deflate"));
        }

        /// <summary>
        /// Sets up the current page or handler to use GZip through a Response.Filter
        ///             IMPORTANT:
        ///             You have to call this method before any output is generated!
        /// 
        /// </summary>
        public static void GZipEncodePage()
        {
            HttpResponse response = HttpContext.Current.Response;
            if (WebUtils.IsGZipSupported())
            {
                if (HttpContext.Current.Request.Headers["Accept-Encoding"].Contains("gzip"))
                {
                    response.Filter = (Stream)new GZipStream(response.Filter, CompressionMode.Compress);
                    response.Headers.Remove("Content-Encoding");
                    response.AppendHeader("Content-Encoding", "gzip");
                }
                else
                {
                    response.Filter = (Stream)new DeflateStream(response.Filter, CompressionMode.Compress);
                    response.Headers.Remove("Content-Encoding");
                    response.AppendHeader("Content-Encoding", "deflate");
                }
            }
            response.AppendHeader("Vary", "Content-Encoding");
        }

        /// <summary>
        /// Translates the current ASP.NET path
        ///             into an application relative path: subdir/page.aspx. The
        ///             path returned is based of the application base and
        ///             starts either with a subdirectory or page name (ie. no ~)
        /// 
        ///             This version uses the current ASP.NET path of the request
        ///             that is active and internally uses AppRelativeCurrentExecutionFilePath
        /// 
        /// </summary>
        /// 
        /// <returns/>
        public static string GetAppRelativePath()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Replace("~/", "");
        }

        /// <summary>
        /// Translates an ASP.NET path like /myapp/subdir/page.aspx
        ///             into an application relative path: subdir/page.aspx. The
        ///             path returned is based of the application base and
        ///             starts either with a subdirectory or page name (ie. no ~)
        /// 
        ///             The path is turned into all lower case.
        /// 
        /// </summary>
        /// <param name="logicalPath">A logical, server root relative path (ie. /myapp/subdir/page.aspx)</param>
        /// <returns>
        /// Application relative path (ie. subdir/page.aspx)
        /// </returns>
        public static string GetAppRelativePath(string logicalPath)
        {
            logicalPath = logicalPath.ToLower();
            string str1 = string.Empty;
            if (HttpContext.Current != null)
            {
                string str2 = HttpContext.Current.Request.ApplicationPath.ToLower();
                if (str2 != "/")
                {
                    string oldValue = str2 + "/";
                    return logicalPath.Replace(oldValue, "");
                }
                else
                    return logicalPath.TrimStart('/');
            }
            else
                return logicalPath.TrimStart('/');
        }

    }
}
