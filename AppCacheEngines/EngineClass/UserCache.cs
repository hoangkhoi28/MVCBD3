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
    public class UserCache
    {
        private static int CACHE_DURATION = Global.TryParseInt(ConfigurationSettings.AppSettings["UserCacheDuration"], 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static User Get(int userID)
        {
            string CacheKey = string.Format("UserCacheDuration_UserID_{0}", userID);

            User _user = (User)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_user == null)
            {
                _user = new User(userID);

                if (_user != null)
                {
                    // Less than 6 hours
                    CMSDataCache.Insert(CacheKey, _user, CACHE_DURATION);
                }
            }

            return _user;
        }

        /// <summary>
        /// Getlist
        /// </summary>
        /// <returns></returns>
        public static List<User> Getlist()
        {
            string CacheKey = string.Format("UserCacheDuration_Getlist");

            List<User> _getlist = (List<User>)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_getlist == null)
            {
                _getlist = new List<User> { };

                try
                {
                    UserRepository obj = new UserRepository();
                    for (int i = 0; i < obj.GetList().Count; i++)
                    {
                        _getlist.Add(UserCache.Get(obj.GetList()[i].ID));
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
        public static List<User> Getlist(string KeySearch, out int total, int pageCount, int pageIndex)
        {
            string CacheKey = string.Format("UserCacheDuration_Getlist_KeySearch_{0}", KeySearch);
            total = 0;
            List<User> _getlist = (List<User>)CMSDataCache.Get(CacheKey);
            // Caching...
            if (_getlist == null)
            {
                _getlist = new List<User> { };

                try
                {
                    UserRepository obj = new UserRepository();
                    var list = obj.GetList(KeySearch, out total, pageCount, pageIndex);
                    for (int i = 0; i < list.Count; i++)
                    {
                        _getlist.Add(UserCache.Get(list[i].ID));
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
        /// get email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static User Get(string Email)
        {
            string CacheKey = string.Format("UserCacheDuration_Email_{0}", Email);

            User _getUser = (User)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_getUser == null)
            {
                _getUser = new User() { };

                try
                {
                    _getUser = UserCache.Getlist().Where
                                (a => a.Email == Email)
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

        public static User GetCheckexists(string username)
        {
            string CacheKey = string.Format("UserCacheDuration_username_{0}", username);

            User _getUser = (User)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_getUser == null)
            {
                _getUser = new User() { };

                try
                {
                    _getUser = UserCache.Getlist().Where
                                (a => a.UserName == username)
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
        /// login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static User Get(string username, string password)
        {
            string CacheKey = string.Format("UserCacheDuration_username_{0}_password_{1}", username, password);

            User _getUser = (User)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_getUser == null)
            {
                _getUser = new User() { };

                try
                {
                    _getUser = UserCache.Getlist().Where
                                (a => a.UserName == username && a.Password == password)
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
        /// Remove from cache
        /// </summary>
        /// <param name="userID"></param>
        public static void Remove(int userID)
        {
            string CacheKey = string.Format("UserCacheDuration_UserID_{0}", userID);

            CMSDataCache.Remove(CacheKey);
        }
    }
}
