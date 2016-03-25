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
    public class DVHCModels
    {

        public DVHC DVHCModel { get; set; }
        public string StringSearch { get; set; }
        public List<DVHC> ListDVHC { get; set; }

        //Paging setting
        public int total { set; get; }
        public int length { set; get; }        
        public int RowIndex { get; set; }
        public int TotalRowCount { get; set; }       
       
        public string strMessage { get; set; }

    }

}
