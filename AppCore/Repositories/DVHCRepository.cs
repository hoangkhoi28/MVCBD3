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
    public class DVHCRepository : AppRepository.IRepositories<DVHC>
    {
        private MvcAppBd3DBEntities context = new MvcAppBd3DBEntities();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DVHC> GetList()
        {
            return context.DVHCs.ToList();
        }
        private IDbSet<DVHC> Dbset
        {
            get { return context.Set<DVHC>(); }
        }

        public IEnumerable<DVHC> GetAll()
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
        public List<DVHC> GetList(string KeyString, out int total, int pageCount, int pageIndex)
        {
            var Dbset = context.Set<DVHC>().AsEnumerable();
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
        public DVHC Get(int id)
        {
            return context.DVHCs.FirstOrDefault(w => w.IDDVHC == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Site"></param>
        /// <returns></returns>
        public bool Save(DVHC _DVHC)
        {
            bool checkProcess = false;
            try
            {
                // Get Site form DB
                DVHC itemToUpdate = context.DVHCs.FirstOrDefault(w => w.IDDVHC == _DVHC.IDDVHC);

                // Update Execute
                if (itemToUpdate != null)
                {
                    itemToUpdate.MoTaDVHC = _DVHC.MoTaDVHC;
                    itemToUpdate.ParentID = _DVHC.ParentID;
                    itemToUpdate.Status = _DVHC.Status;
                    itemToUpdate.TenDVHC = _DVHC.TenDVHC;                    
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
        public int Create(DVHC _DVHC)
        {
            context.DVHCs.Add(_DVHC);
            try
            {
                context.Entry(_DVHC).State = EntityState.Added;
                context.SaveChanges();
            }
            catch
            {
                throw new Exception();
            }
            return _DVHC.IDDVHC;
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
                var _DVHC = context.DVHCs.Where(w => w.IDDVHC == id).FirstOrDefault();
                context.DVHCs.Remove(_DVHC);
                context.Entry(_DVHC).State = EntityState.Deleted;
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
