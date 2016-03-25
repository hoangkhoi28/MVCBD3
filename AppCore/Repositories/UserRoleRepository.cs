using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRepository;
using AppCore.Models;
using System.Data.Entity;

namespace AppCore.Repositories
{
    public class UserRoleRepository : AppRepository.IRepositories<UserRole>
    {
        private MvcAppBd3DBEntities context = new MvcAppBd3DBEntities();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserRole> GetList()
        {
            return context.UserRoles.ToList();
        }

        private IDbSet<UserRole> Dbset
        {
            get { return context.Set<UserRole>(); }
        }

        public IEnumerable<UserRole> GetAll()
        {
            return Dbset.AsEnumerable();
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="KeyString"></param>
        /// <param name="total"></param>
        /// <param name="pageCount"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<UserRole> GetList(string KeyString, out int total, int pageCount, int pageIndex)
        {
            var Dbset = context.Set<UserRole>().AsEnumerable();
            total = Dbset.Count();
            Dbset = Dbset.Skip(pageCount * (pageIndex - 1))
                            .Take(pageCount);

            return Dbset.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserRole Get(int id)
        {
            return context.UserRoles.FirstOrDefault(w => w.ID == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Site"></param>
        /// <returns></returns>
        public bool Save(UserRole _UserRole)
        {
            bool checkProcess = false;
            try
            {
                // Get Site form DB
                UserRole itemToUpdate = context.UserRoles.FirstOrDefault(w => w.ID == _UserRole.ID);

                // Update Execute
                if (itemToUpdate != null)
                {
                    itemToUpdate.RoleName = _UserRole.RoleName;
                    itemToUpdate.ShortName = _UserRole.ShortName;
                    itemToUpdate.Description = _UserRole.Description;
                    itemToUpdate.ModifiedDate = DateTime.Now;                    
                }

                // Save Changes to DB
                context.Entry(itemToUpdate).State = EntityState.Modified;
                context.SaveChanges();
                checkProcess = true;
            }
            catch (Exception)
            {
                checkProcess = false;
            }

            return checkProcess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Site"></param>
        /// <returns></returns>
        public int Create(UserRole _UserRole)
        {
            context.UserRoles.Add(_UserRole);
            try
            {
                context.Entry(_UserRole).State = EntityState.Added;
                context.SaveChanges();
            }
            catch
            {
                throw new Exception();
            }
            return _UserRole.ID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool checkProcess = false;
            try
            {
                var _userRole = context.UserRoles.Where(w => w.ID == id).FirstOrDefault();
                context.UserRoles.Remove(_userRole);
                context.Entry(_userRole).State = EntityState.Deleted;
                context.SaveChanges();

                checkProcess = true;
            }
            catch (Exception)
            {
                checkProcess = false;
            }
            return checkProcess;
        }

    }
}
