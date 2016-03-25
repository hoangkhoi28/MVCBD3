using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Repositories;

namespace AppCacheEngines.EntitiesClass
{
    public class UserRole
    {
        #region properties
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        #endregion

        #region contructor

        /// <summary>
        /// 
        /// </summary>
        public UserRole()
        {
            ID = 0;
            RoleName = "";
            ShortName = "";
            Description = "";
            ModifiedDate = DateTime.Now;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserRoleID"></param>
        public UserRole(int UserRoleID)
        {
            UserRoleRepository objRole = new UserRoleRepository();
            var _UserRole = objRole.Get(UserRoleID);
            if (_UserRole != null)
            {
                this.ID = _UserRole.ID;
                this.RoleName = _UserRole.RoleName;
                this.ShortName = _UserRole.ShortName;
                this.Description = _UserRole.Description;
                this.ModifiedDate = _UserRole.ModifiedDate;
            }
        }
        #endregion
    }
}
