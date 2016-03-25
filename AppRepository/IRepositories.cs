using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository
{   
    public interface IRepositories<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<T> GetList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_T"></param>
        /// <returns></returns>
        bool Save(T _T);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_T"></param>
        /// <returns></returns>
        int Create(T _T);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
    }
}
