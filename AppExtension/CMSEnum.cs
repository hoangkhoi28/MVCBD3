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

namespace AppExtension
{
    /// <summary>
    /// 
    /// </summary>
    public enum StatusCommon
    {
        Waiting = 1,
        Approved = 2,
        Locked = 3,
        Deleted = 4
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FileType
    {
        Image = 1,
        Audio = 2,
        Video = 3,
        Flash = 4,
        Document = 5
    }

   
}