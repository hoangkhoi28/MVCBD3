using AppCacheEngines.EntitiesClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcAppBd3Web.Models
{
    public class AccountModels
    {

        public User userModel { get; set; }
        public bool RememberMe { get; set; }
        public string confirmpassword { get; set; }
        public string StringSearch { get; set; }
        public List<User> ListUser { get; set; }

        //Paging setting
        public int total { set; get; }
        public int length { set; get; }        
        public int RowIndex { get; set; }
        public int TotalRowCount { get; set; }
        
        //create
        public List<UserRole> ListUserRole { get; set; }
        public SelectList selectUserRole { get; set; }
        public string strRole { get; set; }
        public List<DVHC> ListDVHC { get; set; }
        public IEnumerable selectDVHC { get; set; }
        public string strDVHC { get; set; }
        public string strMessage { get; set; }

    }

}
