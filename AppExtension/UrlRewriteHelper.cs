using System;
using System.Web.Caching;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Configuration;
using System.Collections.Generic;
using System.Xml;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AppExtension
{
    /// <summary>
    /// Summary description for UrlRewriteHelper
    /// </summary>
    public class UrlRewriteHelper
    {
        const string FindText = "ỵỹỷỳýựữửừứưụũủùúợỡởờớơộỗổồốôọõỏòóịĩỉìíệễểềếêẹẽẻèéđậẫẩầấâặẵẳằắăạãảàáỴỸỶỲÝỰỮỬỪỨƯỤŨỦÙÚỢỠỞỜỚƠỘỖỔỒỐÔỌÕỎÒÓỊĨỈÌÍỆỄỂỀẾÊẸẼẺÈÉĐẬẪẨẦẤÂẶẴẲẰẮĂẠÃẢÀÁ ’\".$`~!@'#%^&*()?/\\>,<;:_+";
        const string ReplText = "yyyyyuuuuuuuuuuuoooooooooooooooooiiiiieeeeeeeeeeedaaaaaaaaaaaaaaaaaYYYYYUUUUUUUUUUUOOOOOOOOOOOOOOOOOIIIIIEEEEEEEEEEEDAAAAAAAAAAAAAAAAA-";

        public static string UnicodeToUnsigned(string s)
        {
            string uniChars = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";
            string KoDauChars = "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";

            string retVal = String.Empty;
            int pos;
            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += KoDauChars[pos];
                else
                    retVal += s[i];
            }
            retVal = retVal.Replace("–", " ");
            retVal = retVal.Replace("-", " ");
            retVal = retVal.Replace("  ", " ");
            retVal = retVal.Replace(" ", " ");
            retVal = retVal.Replace("--", " ");
            retVal = retVal.Replace(":", " ");
            retVal = retVal.Replace(";", " ");
            retVal = retVal.Replace("+", " ");
            retVal = retVal.Replace("@", " ");
            retVal = retVal.Replace(">", " ");
            retVal = retVal.Replace("<", " ");
            retVal = retVal.Replace("*", " ");
            retVal = retVal.Replace("{", " ");
            retVal = retVal.Replace("}", " ");
            retVal = retVal.Replace("|", " ");
            retVal = retVal.Replace("^", " ");
            retVal = retVal.Replace("~", " ");
            retVal = retVal.Replace("]", " ");
            retVal = retVal.Replace("[", " ");
            retVal = retVal.Replace("`", " ");
            retVal = retVal.Replace(".", " ");
            retVal = retVal.Replace("'", " ");
            retVal = retVal.Replace("(", " ");
            retVal = retVal.Replace(")", " ");
            retVal = retVal.Replace(",", " ");
            retVal = retVal.Replace("”", " ");
            retVal = retVal.Replace("“", " ");
            retVal = retVal.Replace("?", " ");
            retVal = retVal.Replace("\"", " ");
            retVal = retVal.Replace("&", " ");
            retVal = retVal.Replace("$", " ");
            retVal = retVal.Replace("#", " ");
            retVal = retVal.Replace("_", " ");
            retVal = retVal.Replace("=", " ");
            retVal = retVal.Replace("%", " ");
            retVal = retVal.Replace("…", " ");
            retVal = retVal.Replace("/", " ");
            retVal = retVal.Replace("\\", " ");
            retVal = retVal.Replace(" ", "-");
            retVal = retVal.Replace("--", "-");
            retVal = retVal.Replace("---", "-");
            retVal = retVal.Replace("----", "-");
            retVal = retVal.Replace("-----", "-");
            return retVal.ToLower().TrimEnd('-').TrimStart('-');
        }

        /// <summary>
        /// Get ObjectID From Rewrited Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int GetObjectIDFromUrl(string url)
        {
            // Parse URL to get (PageName, PageID)
            // *** Case 1: News/Sport/Index.html
            // *** Case 2: News/Sport/6868/Article_Title.html        
            string relativeUrl = VirtualPathUtility.ToAppRelative(url).Substring(2);
            string[] urlSegments = relativeUrl.Split(new char[] { '/' });
            string strObjectID = urlSegments[urlSegments.Length - 2];
            int objectID = -1;
            try
            {
                objectID = int.Parse(strObjectID);
            }
            catch
            {
                objectID = -1;
            }

            return objectID;
        }

        /// <summary>
        /// Convert Title, Name to Url Standard
        /// </summary>
        /// <param name="strVietNamese"></param>
        /// <returns></returns>
        public static string ConvertTitle2Url(string strVietNamese)
        {
            if (string.IsNullOrEmpty(strVietNamese))
            {
                return "Index.html";
            }

            // Decode HTML Character
            strVietNamese = HttpUtility.HtmlDecode(strVietNamese);

            int index = -1;
            while ((index = strVietNamese.IndexOfAny(FindText.ToCharArray())) != -1)
            {
                int index2 = FindText.IndexOf(strVietNamese[index]);
                if (index2 > 134)
                {
                    strVietNamese = strVietNamese.Remove(index, 1);
                }
                else
                {
                    strVietNamese = strVietNamese.Replace(strVietNamese[index], ReplText[index2]);
                }
            }

            return strVietNamese + ".html";
        }        

   

        /// <summary>
        /// Get standard Rewrited Url for Article    
        /// </summary>
        /// <param name="ArticleID"></param>
        /// <returns></returns>
        //public static string GetArticleSimpleUrl(int articleID)
        //{
        //    string CacheKey = string.Format("ArticleSimpleUrl_ArticleID_{0}", articleID);
        //    string articleUrl = (string)CMSDataCache.Get(CacheKey);

        //    // Caching...
        //    if (articleUrl == null)
        //    {
        //        try
        //        {
        //            string title = "";
        //            ArticleFull _ArticleFull = new ArticleFull(articleID);
        //            title = _ArticleFull.Title.Trim();
        //            articleUrl = articleID + "/" + ConvertTitle2Url(title);

        //            // Insert Into Cache
        //            if (articleUrl != null)
        //            {
        //                CMSDataCache.Insert(CacheKey, articleUrl, Global.UrlRewriteCacheDuration);
        //            }
        //        }
        //        catch { }
        //    }

        //    return articleUrl;
        //}

        public static string ConvertToUnSign(string text)
        {
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            text = text.Replace(" ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string ConvertToUnSignArticle(string text)
        {          
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            string retVal =  regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            retVal = retVal.Replace("–", " ");
            retVal = retVal.Replace("-", " ");
            retVal = retVal.Replace("  ", " ");
            retVal = retVal.Replace(" ", " ");
            retVal = retVal.Replace("--", " ");
            retVal = retVal.Replace(":", " ");
            retVal = retVal.Replace(";", " ");
            retVal = retVal.Replace("+", " ");
            retVal = retVal.Replace("@", " ");
            retVal = retVal.Replace(">", " ");
            retVal = retVal.Replace("<", " ");
            retVal = retVal.Replace("*", " ");
            retVal = retVal.Replace("{", " ");
            retVal = retVal.Replace("}", " ");
            retVal = retVal.Replace("|", " ");
            retVal = retVal.Replace("^", " ");
            retVal = retVal.Replace("~", " ");
            retVal = retVal.Replace("]", " ");
            retVal = retVal.Replace("[", " ");
            retVal = retVal.Replace("`", " ");
            retVal = retVal.Replace(".", " ");
            retVal = retVal.Replace("'", " ");
            retVal = retVal.Replace("(", " ");
            retVal = retVal.Replace(")", " ");
            retVal = retVal.Replace(",", " ");
            retVal = retVal.Replace("”", " ");
            retVal = retVal.Replace("“", " ");
            retVal = retVal.Replace("?", " ");
            retVal = retVal.Replace("\"", " ");
            retVal = retVal.Replace("&", " ");
            retVal = retVal.Replace("$", " ");
            retVal = retVal.Replace("#", " ");
            retVal = retVal.Replace("_", " ");
            retVal = retVal.Replace("=", " ");
            retVal = retVal.Replace("%", " ");
            retVal = retVal.Replace("…", " ");
            retVal = retVal.Replace("/", " ");
            retVal = retVal.Replace("\\", " ");
            retVal = retVal.Replace(" ", "-");
            retVal = retVal.Replace("--", "-");
            retVal = retVal.Replace("---", "-");
            retVal = retVal.Replace("----", "-");
            retVal = retVal.Replace("-----", "-");
            return retVal.ToLower().TrimEnd('-').TrimStart('-');
        }

    }
}