using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;

namespace AppExtension
{
    /// <summary>
    /// Summary description for Global
    /// </summary>
    public class Global
    {
        // Connection String
        public static string CMSConnectionString = ConfigurationManager.ConnectionStrings["MvcAppBd3DBEntities"].ConnectionString;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int TryParseInt(string source, int defaultValue)
        {
            try
            {
                int intResult = defaultValue;
                int.TryParse(source, out intResult);
                return intResult;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="ClassName"></param>
        /// <param name="RemoveFormatting"></param>
        /// <returns></returns>
        public static string GetParagraph(string Content, string ClassName, bool RemoveFormatting)
        {
            string str = Content.ToLower();
            string str2 = ClassName.ToLower();
            int startIndex = 0;
            int index = 0;
            while ((startIndex != -1) && (index != -1))
            {
                startIndex = str.IndexOf("<p class=" + str2, index);
                if (startIndex != -1)
                {
                    index = str.IndexOf("</p>", startIndex);
                    int num3 = Content.IndexOf("<p", startIndex + 1, (int)(index - startIndex));
                    if (num3 != -1)
                    {
                        index = num3;
                    }
                    if (index != -1)
                    {
                        string str3 = RemoveFormatting ? StripHtml(Content.Substring(startIndex, index - startIndex)) : StripParagraph(Content.Substring(startIndex, index - startIndex));
                        if (str3.Length != 0)
                        {
                            return str3;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static string StripParagraph(string S)
        {
            int index = S.IndexOf(">");
            if (index != -1)
            {
                return S.Substring(index + 1).Trim();
            }
            return S.Trim();
        }

        public static string StripHtml(string S)
        {
            try
            {
                int startIndex = 0;
                int num2 = 0;
                while (((startIndex = S.IndexOf("<", startIndex)) != -1) && ((num2 = S.IndexOf(">", startIndex)) != -1))
                {
                    S = S.Remove(startIndex, (num2 - startIndex) + 1);
                }
            }
            catch
            {
                S = "Đang cập nhật...";
            }
            return S.Trim();
        }

        public static string EscapeQuote(DateTime d)
        {
            return d.ToString(@"\'yyyy-MM-dd HH:mm:ss\'");
        }

        public static string EscapeQuote(string s)
        {
            return ("'" + s.Trim().Replace("'", "''") + "'");
        }

        /// <summary>
        /// Utanyon - HtmlHelper
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public static string EscapeQuoteHtml(string S)
        {
            if (S == null)
                return "''";
            //			else
            //				return "'" + S.Trim().Replace("'", "\\'").Replace("\r\n", "\\r\\n") + "'";
            StringBuilder sb = new StringBuilder("'", S.Length + 2);
            foreach (char c in S)
            {
                if (c == '\'')
                    sb.Append("\\'");
                else if (c == '\r')
                    sb.Append("\\r");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c > '~')
                    sb.Append("&#" + Convert.ToInt16(c) + ";");
                else
                    sb.Append(c);
            }
            sb.Append("'");
            return sb.ToString();
        }

        public static string EscapeQuoteButNotTrim(string s)
        {
            return ("'" + s.Replace("'", "''") + "'");
        }

        public static string EscapeQuoteUnicode(string s)
        {
            return ("N'" + s.Trim().Replace("'", "''") + "'");
        }

        public static string GetString(object source, string defaultValue)
        {
            if (!IsNull(source))
            {
                return (string)source;
            }
            return defaultValue;
        }

        public static bool IsNull(object obj)
        {
            return ((obj == null) || (obj is DBNull));
        }

        public static int GetInt(object source, int defaultValue)
        {
            if (!IsNull(source))
            {
                return Convert.ToInt32(source);
            }
            return defaultValue;
        }

        /// <summary>
        /// Cut String Function
        /// </summary>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string cutStrExper(object name, int length)
        {
            try
            {
                if (name != null)
                {
                    string title = name.ToString();

                    if (title == "?")
                        return string.Empty;

                    if (title.Length <= length)
                    {
                        return title;
                    }
                    else
                    {
                        // except 3 character ... 
                        length = length - 3;

                        title = title.Substring(0, length);
                        title = title.Substring(0, title.LastIndexOf(" "));
                        title = title + "...";
                        return title;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
      
    }  
}