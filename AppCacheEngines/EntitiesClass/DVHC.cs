using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Repositories;

namespace AppCacheEngines.EntitiesClass
{
    public class DVHC
    {
        #region properties
        public int IDDVHC { get; set; }
        public string TenDVHC { get; set; }
        public string MoTaDVHC { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> ParentID { get; set; }

        #endregion

        #region contructor

        /// <summary>
        /// 
        /// </summary>
        public DVHC()
        {
            IDDVHC = 0;
            TenDVHC = "";
            MoTaDVHC = "";
            Status = 0;
            ParentID = 0;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDDVHC"></param>
        public DVHC(int _IDDVHC)
        {
            DVHCRepository objRole = new DVHCRepository();
            var _DVHC = objRole.Get(_IDDVHC);
            if (_DVHC != null)
            {
                this.IDDVHC = _DVHC.IDDVHC;
                this.TenDVHC = _DVHC.TenDVHC;
                this.MoTaDVHC = _DVHC.MoTaDVHC;
                this.Status = _DVHC.Status;
                this.ParentID = _DVHC.ParentID;
            }
        }
        #endregion
    }
}
