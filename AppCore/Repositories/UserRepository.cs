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
    public class UserRepository : AppRepository.IRepositories<User>
    {
        private MvcAppBd3DBEntities context = new MvcAppBd3DBEntities();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<User> GetList()
        {
            return context.Users.ToList();
        }

        private IDbSet<User> Dbset
        {
            get { return context.Set<User>(); }
        }

        public IEnumerable<User> GetAll()
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
        public List<User> GetList(string KeyString, out int total, int pageCount, int pageIndex)
        {
            var Dbset = context.Set<User>().AsEnumerable();
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
        public User Get(int id)
        {
            return context.Users.FirstOrDefault(w => w.ID == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_User"></param>
        /// <returns></returns>
        public bool Save(User _User)
        {
            bool checkProcess = false;
            try
            {
                // Get Site form DB
                User itemToUpdate = context.Users.FirstOrDefault(w => w.ID == _User.ID);

                // Update Execute
                if (itemToUpdate != null)
                {
                    itemToUpdate.Account = _User.Account;
                    itemToUpdate.Password = _User.Password;
                    itemToUpdate.Email = _User.Email;
                    itemToUpdate.Modified = DateTime.Now;
                    itemToUpdate.IDRole = _User.IDRole;
                    itemToUpdate.Status = _User.Status;
                    itemToUpdate.IDDVHC = _User.IDDVHC;
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
        /// <param name="_User"></param>
        /// <returns></returns>
        public int Create(User _User)
        {
            context.Users.Add(_User);
            try
            {
                context.Entry(_User).State = EntityState.Added;
                context.SaveChanges();
            }
            catch
            {
                throw new Exception();
            }
            return _User.ID;
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
                var _User = context.Users.Where(w => w.ID == id).FirstOrDefault();
                context.Users.Remove(_User);
                context.Entry(_User).State = EntityState.Deleted;
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
