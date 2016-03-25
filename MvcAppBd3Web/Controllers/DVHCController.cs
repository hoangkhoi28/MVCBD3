using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MvcAppBd3Web.Models;
using AppCacheEngines.EngineClass;
using System.Configuration;
using System.Web.Mail;
using System.Net;
using System.Net.Mail;
using AppExtension;
using AppCacheEngines.EntitiesClass;
using AppCore.Repositories;

namespace MvcAppBd3Web.Controllers
{

    public class DVHCController : Controller
    {
        #region Xử lý công việc : danh sách, thêm, sua
        public ActionResult Index()
        {
            DVHCModels modelSearch = new DVHCModels();
            modelSearch.length = 10;
            modelSearch.RowIndex = 1;
            modelSearch.StringSearch = "";
            int totalRow = 0;
            //modelSearch.ListUser = UserCache.Getlist(modelSearch.StringSearch, out totalRow, modelSearch.length, modelSearch.RowIndex);
            modelSearch.ListDVHC = DVHCCache.Getlist();
            modelSearch.total = totalRow;

            return View(modelSearch);
        }

        /// <summary>
        /// Action init
        /// </summary>
        /// <param name="id">or</param>
        /// <returns></returns>
        public ActionResult Action(int? id)
        {
            var model = new DVHCModels();
            #region setup model
            model.DVHCModel = new AppCacheEngines.EntitiesClass.DVHC();
            if (id.HasValue)
            {
                DVHCRepository OBJ = new DVHCRepository();
                AppCore.Models.DVHC _DVHC = OBJ.Get(id.Value);
                model.DVHCModel.IDDVHC = _DVHC.IDDVHC;
                model.DVHCModel.MoTaDVHC = _DVHC.MoTaDVHC;
                model.DVHCModel.TenDVHC = _DVHC.TenDVHC;
                return View(model);
            }
            #endregion
            return View(model);
        }

        /// <summary>
        /// Action create, udpate post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Action(DVHCModels model)
        {
            DVHCRepository OBJ = new DVHCRepository();
            AppCore.Models.DVHC _DVHC = new AppCore.Models.DVHC();
            if (model.DVHCModel.IDDVHC == 0)
            {
                #region create
                var userrole = DVHCCache.GetCheckexists(model.DVHCModel.TenDVHC);
                if (userrole == null)
                {
                    _DVHC.TenDVHC = model.DVHCModel.TenDVHC;
                    _DVHC.ParentID = null;
                    _DVHC.Status = (int)StatusCommon.Approved;
                    _DVHC.MoTaDVHC = model.DVHCModel.MoTaDVHC;
                    int statuscreate = OBJ.Create(_DVHC);
                    if (statuscreate > 0)
                    {
                        model.strMessage = "Đã tạo đơn vị hành chính thành công!";
                    }
                    else
                    {
                        model.strMessage = "Có lỗi xảy ra!";
                    }
                }
                else
                {
                    model.strMessage = "Đơn vị hành chính này đã có trong hệ thống!";
                }
                #endregion
            }
            else
            {
                #region update
                _DVHC.IDDVHC = model.DVHCModel.IDDVHC;
                _DVHC.TenDVHC = model.DVHCModel.TenDVHC;
                _DVHC.ParentID = null;
                _DVHC.Status = (int)StatusCommon.Approved;
                _DVHC.MoTaDVHC = model.DVHCModel.MoTaDVHC;
                bool statussave = OBJ.Save(_DVHC);
                if (statussave)
                {
                    model.strMessage = "Đã sửa đơn vị hành chính thành công!";
                }
                else
                {
                    model.strMessage = "Có lỗi xảy ra!";
                }
                #endregion
            }

            #region setup model


            #endregion
            return View(model);
        }

        #endregion
    }
}
