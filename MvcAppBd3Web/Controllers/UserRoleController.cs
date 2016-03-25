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

    public class UserRoleController : Controller
    {
        #region Xử lý công việc : danh sách, thêm, sua
        public ActionResult Index()
        {
            UserRoleModels modelSearch = new UserRoleModels();
            modelSearch.length = 10;
            modelSearch.RowIndex = 1;
            modelSearch.StringSearch = "";
            int totalRow = 0;
            //modelSearch.ListUser = UserCache.Getlist(modelSearch.StringSearch, out totalRow, modelSearch.length, modelSearch.RowIndex);
            modelSearch.ListUserRole = UserRoleCache.Getlist();
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
            var model = new UserRoleModels();
            #region setup model
            model.userRoleModel = new AppCacheEngines.EntitiesClass.UserRole();
            if (id.HasValue)
            {
                UserRoleRepository OBJ = new UserRoleRepository();
                AppCore.Models.UserRole _userRole = OBJ.Get(id.Value);
                model.userRoleModel.ID = _userRole.ID;
                model.userRoleModel.RoleName = _userRole.RoleName;
                model.userRoleModel.Description = _userRole.Description;
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
        public ActionResult Action(UserRoleModels model)
        {
            UserRoleRepository OBJ = new UserRoleRepository();
            AppCore.Models.UserRole _userRole = new AppCore.Models.UserRole();
            if (model.userRoleModel.ID == 0)
            {
                #region create
                var userrole = UserRoleCache.GetCheckexists(model.userRoleModel.RoleName);
                if (userrole == null)
                {
                    _userRole.RoleName = model.userRoleModel.RoleName;
                    _userRole.Description = model.userRoleModel.Description;
                    _userRole.ModifiedDate = DateTime.Now;
                    int statuscreate = OBJ.Create(_userRole);
                    if (statuscreate > 0)
                    {
                        model.strMessage = "Đã tạo quyền thành công!";
                    }
                    else
                    {
                        model.strMessage = "Có lỗi xảy ra!";
                    }
                }
                else
                {
                    model.strMessage = "Quyền này đã có trong hệ thống!";
                }
                #endregion
            }
            else
            {
                #region update
                _userRole.ID = model.userRoleModel.ID;
                _userRole.RoleName = model.userRoleModel.RoleName;
                _userRole.Description = model.userRoleModel.Description;
                _userRole.ModifiedDate = DateTime.Now;
                bool statussave = OBJ.Save(_userRole);
                if (statussave)
                {
                    model.strMessage = "Đã sửa quyền thành công!";
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
