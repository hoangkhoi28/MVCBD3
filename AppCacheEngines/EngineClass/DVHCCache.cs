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
    public class DVHCCache
    {
        private static int CACHE_DURATION = Global.TryParseInt(ConfigurationSettings.AppSettings["DVHCCacheDuration"], 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DVHCID"></param>
        /// <returns></returns>
        public static DVHC Get(int DVHCID)
        {
            string CacheKey = string.Format("DVHCCacheDuration_DVHCID_{0}", DVHCID);

            DVHC _DVHC = (DVHC)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_DVHC == null)
            {
                _DVHC = new DVHC(DVHCID);

                if (_DVHC != null)
                {
                    // Less than 6 hours
                    CMSDataCache.Insert(CacheKey, _DVHC, CACHE_DURATION);
                }
            }

            return _DVHC;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public static DVHC GetCheckexists(string DVHCName)
        {
            string CacheKey = string.Format("DVHCCacheDuration_username_{0}", DVHCName);

            DVHC _getDVHC = (DVHC)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_getDVHC == null)
            {
                _getDVHC = new DVHC() { };

                try
                {
                    _getDVHC = DVHCCache.Getlist().Where
                                (a => a.TenDVHC == DVHCName)
                                .FirstOrDefault();
                }
                catch { }

                if (_getDVHC != null)
                {
                    CMSDataCache.Insert(CacheKey, _getDVHC, CACHE_DURATION);
                }
            }

            return _getDVHC;
        }
        /// <summary>
        /// Getlist
        /// </summary>
        /// <returns></returns>
        public static List<DVHC> Getlist()
        {
            string CacheKey = string.Format("DVHCCacheDuration_Getlist");

            List<DVHC> _getlist = (List<DVHC>)CMSDataCache.Get(CacheKey);

            // Caching...
            if (_getlist == null)
            {
                _getlist = new List<DVHC> { };

                try
                {
                    DVHCRepository obj = new DVHCRepository();
                    for (int i = 0; i < obj.GetList().Count; i++)
                    {
                        _getlist.Add(DVHCCache.Get(obj.GetList()[i].IDDVHC));
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
        public static List<DVHC> Getlist(string KeySearch, out int total, int pageCount, int pageIndex)
        {
            string CacheKey = string.Format("DVHCCacheDuration_Getlist_KeySearch_{0}", KeySearch);
            total = 0;
            List<DVHC> _getlist = (List<DVHC>)CMSDataCache.Get(CacheKey);
            // Caching...
            if (_getlist == null)
            {
                _getlist = new List<DVHC> { };

                try
                {
                    DVHCRepository obj = new DVHCRepository();
                    var list = obj.GetList(KeySearch, out total, pageCount, pageIndex);
                    for (int i = 0; i < list.Count; i++)
                    {
                        _getlist.Add(DVHCCache.Get(list[i].IDDVHC));
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
        /// <param name="DVHCID"></param>
        public static void Remove(int DVHCID)
        {
            string CacheKey = string.Format("DVHCCacheDuration_DVHCID_{0}", DVHCID);

            CMSDataCache.Remove(CacheKey);
        }
    }
}
