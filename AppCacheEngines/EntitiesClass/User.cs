using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Repositories;

namespace AppCacheEngines.EntitiesClass
{
    public class User
    {
        #region properties
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? Modified { get; set; }
        public int? IDRole { get; set; }
        public int? Status { get; set; }
        public int? IDDVHC { get; set; }
        public int Total { get; set; }
        public string RoleName { get; set; }
        public string NameDVHC { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int IDTinh { get; set; }
        public int IDHuyen { get; set; }
        public int IDXa { get; set; }
        #endregion

        #region contructor

        /// <summary>
        /// 
        /// </summary>
        public User()
        {
            ID = 0;
            UserName = "";
            Password = "";
            Email = "";
            Modified = DateTime.Now;
            IDRole = 1;
            Status = 1;
            IDDVHC = 1;
            RoleName = "";
            NameDVHC = "";
            FullName = "";
            PhoneNumber = "";
            Address = "";
            IDTinh = 0;
            IDHuyen = 0;
            IDXa = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        public User(int userID)
        {
            UserRepository obj = new UserRepository();
            UserRoleRepository objRole = new UserRoleRepository();
            DVHCRepository objdvhc = new DVHCRepository();
            var User = obj.Get(userID);
            if (User != null)
            {
                this.ID = User.ID;
                this.UserName = User.Account;
                this.Password = User.Password;
                this.Email = User.Email;
                this.Modified = User.Modified;
                this.IDRole = User.IDRole;
                this.Status = User.Status;
                this.IDDVHC = User.IDDVHC;
                this.RoleName = objRole.Get(int.Parse(this.IDRole + "")).RoleName;
                this.NameDVHC = objdvhc.Get(int.Parse(this.IDDVHC + "")).TenDVHC;
                this.Modified = User.Modified;
                this.FullName = User.FullName;
                this.PhoneNumber = User.PhoneNumber;
                this.Address = User.Address;
                this.IDTinh = User.IDTinh.HasValue ? User.IDTinh.Value : 0;
                this.IDHuyen = User.IDHuyen.HasValue ? User.IDHuyen.Value : 0;
                this.IDXa = User.IDXa.HasValue ? User.IDXa.Value : 0;

            }
        }
        #endregion
    }
}
