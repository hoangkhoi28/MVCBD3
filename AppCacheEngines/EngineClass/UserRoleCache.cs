using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Repositories;
using AppExtension;
using System.Configuration;
using AppCacheEngines.EntitiesClass;

namespace AppCacheEngines.EngineClass
{
    public class UserRoleCache
    {
        private static int CACHE_DURATION = Global.TryParseInt(ConfigurationSettings.AppSettings["UserRoleCacheDuration"], 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserRoleID"></param>
        /// <returns></returns>
        public static UserRole Get(int UserRoleID)
        {
            string CacheKey = string.Format("UserRoleCacheDuration_UserRoleID_{0}", UserRoleID);

            UserRole _UserRole = (UserRole)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_UserRole == null)
            {
                _UserRole = new UserRole(UserRoleID);

                if (_UserRole != null)
                {
                    // Less than 6 hours
                    CMSDataCache.Insert(CacheKey, _UserRole, CACHE_DURATION);
                }
            }

            return _UserRole;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public static UserRole GetCheckexists(string RoleName)
        {
            string CacheKey = string.Format("UserRoleCacheDuration_username_{0}", RoleName);

            UserRole _getUser = (UserRole)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_getUser == null)
            {
                _getUser = new UserRole() { };

                try
                {
                    _getUser = UserRoleCache.Getlist().Where
                                (a => a.RoleName == RoleName)
                                .FirstOrDefault();
                }
                catch { }

                if (_getUser != null)
                {
                    CMSDataCache.Insert(CacheKey, _getUser, CACHE_DURATION);
                }
            }

            return _getUser;
        }
        /// <summary>
        /// Getlist
        /// </summary>
        /// <returns></returns>
        public static List<UserRole> Getlist()
        {
            string CacheKey = string.Format("UserRoleCacheDuration_Getlist");

            List<UserRole> _getlist = (List<UserRole>)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_getlist == null)
            {
                _getlist = new List<UserRole> { };

                try
                {
                    UserRoleRepository obj = new UserRoleRepository();
                    for (int i = 0; i < obj.GetList().Count; i++)
                    {
                        _getlist.Add(UserRoleCache.Get(obj.GetList()[i].ID));
                    }
                }
                catch { }

                if (_getlist != null)
                {
                    CMSDataCache.Insert(CacheKey, _getlist, CACHE_DURATION);
                }
            }

            return _getlist;
        }

        /// <summary>
        /// Getlist
        /// </summary>
        /// <returns></returns>
        public static List<UserRole> Getlist(string KeySearch, out int total, int pageCount, int pageIndex)
        {
            string CacheKey = string.Format("UserRoleCacheDuration_Getlist_KeySearch_{0}", KeySearch);
            total = 0;
            List<UserRole> _getlist = (List<UserRole>)CMSDataCache.Get(CacheKey);
            // Caching...
            if (_getlist == null)
            {
                _getlist = new List<UserRole> { };

                try
                {
                    UserRoleRepository obj = new UserRoleRepository();
                    var list = obj.GetList(KeySearch, out total, pageCount, pageIndex);
                    for (int i = 0; i < list.Count; i++)
                    {
                        _getlist.Add(UserRoleCache.Get(list[i].ID));
                    }
                }
                catch { }

                if (_getlist != null)
                {
                    CMSDataCache.Insert(CacheKey, _getlist, CACHE_DURATION);
                }
            }

            return _getlist;
        }

        /// <summary>
        /// Remove from cache
        /// </summary>
        /// <param name="UserRoleID"></param>
        public static void Remove(int UserRoleID)
        {
            string CacheKey = string.Format("UserRoleCacheDuration_UserRoleID_{0}", UserRoleID);

            CMSDataCache.Remove(CacheKey);
        }
    }
}
