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

    public class AccountController : Controller
    {
        #region Xử lý công việc bên ngoài: login, forgot, reset
        public ActionResult Login(string returnUrl)
        {
            if (Request.Cookies["userBD3"] != null)
            {
                var value = Request.Cookies["userBD3"].Value.ToString();
                string[] srtUser = value.Split('}');
                if (srtUser.Length > 1)
                {
                    var userName = srtUser[0].ToString();
                    var passWord = srtUser[1].ToString();
                    var user = UserCache.Get(userName, passWord);
                    if (user != null)
                    {
                        Session["userDisplay"] = user.UserName;
                        return Redirect("/Home/index");
                    }
                }
            }
            if (Session["userlogin"] != null)
            {
                return Redirect("/Home/index");
            }
            return View();

        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(AccountModels model, string returnUrl)
        {
            string pass = Md5Manage.GetMd5Hash(model.userModel.Password);
            var user = UserCache.Get(model.userModel.UserName, pass);
            if (user == null)
            {
                ModelState.AddModelError("error_msg", "Tài khoản hoặc mật khẩu không đúng!");
                return View(model);
            }
            else
            {
                if (user.Status == (int)AppExtension.StatusCommon.Locked)
                {
                    ModelState.AddModelError("error_msg", "Tài khoản này bị khóa.");
                }
                else
                {
                    if (model.RememberMe)
                    {
                        HttpCookie UserCookies = new HttpCookie("userBD3");
                        UserCookies.Value = user.UserName + "}" + model.userModel.Password;
                        UserCookies.Expires = DateTime.Now.AddHours(12);
                        Response.Cookies.Add(UserCookies);
                    }
                    Session["userDisplay"] = user.UserName;
                    return Redirect("/Home/index");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff             
        public ActionResult LogOff()
        {
            if (Request.Cookies["userBD3"] != null)
            {
                Response.Cookies["userBD3"].Expires = DateTime.Now.AddDays(-1);
            }
            Session.Abandon();
            return Redirect("/Account/login");
        }

        /// <summary>
        /// Forgotpassword
        /// </summary>
        /// <returns></returns>
        public ActionResult Forgotpassword()
        {
            return View();
        }

        /// <summary>
        /// Forgotpassword
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Forgotpassword(AccountModels model)
        {
            if (ModelState.IsValid)
            {
                var foundUserName = UserCache.Get(model.userModel.Email);
                if (foundUserName != null)
                {
                    var token = Md5Manage.GetMd5Hash(foundUserName.Password);

                    string resetlink = "<a href='" + Url.Action("ResetPassword", "Home", new { rt = token, account = foundUserName.UserName }, "http") + "'>Reset Password</a>";
                    string subject = null;
                    string body = null;

                    subject = "Thiết lập mật khẩu";
                    body = "Vui lòng bấm liên kết " + resetlink + "để tạo mật khẩu mới";


                    string from = ConfigurationSettings.AppSettings["MailSend"].ToString();
                    string pass = ConfigurationSettings.AppSettings["MailPass"].ToString();
                    string host = ConfigurationSettings.AppSettings["MailHost"].ToString();
                    string port = ConfigurationSettings.AppSettings["MailPort"].ToString();//587

                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, model.userModel.Email);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

                    NetworkCredential netcredential = new NetworkCredential(from, pass);
                    smtp.Host = host;
                    smtp.Port = int.Parse(port);
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = netcredential;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    try
                    {
                        smtp.Send(message);
                        ModelState.AddModelError("", "Liên kết đã gửi đến hòm thư của bạn. Vui lòng kiểm tra hòm thư.");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Vấn đề khi gửi Email :" + e.Message);
                    }
                }

                else
                {
                    ModelState.AddModelError("", "Không tìm thấy người dùng này.");
                }
            }

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public ActionResult ResetPassword(string rt, string account)
        {
            var token = rt;
            var user = UserCache.Get(account, rt);
            if (user != null)
            {
                AccountModels model = new AccountModels();
                model.userModel = user;
                return View(model);
            }
            else
            {
                return Redirect("/Home/index");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(AccountModels model)
        {

            if (ModelState.IsValid)
            {
                var usermodeldata = UserCache.Get(model.userModel.UserName, model.userModel.Password);
                if (usermodeldata != null)
                {
                    model.userModel.Password = Md5Manage.GetMd5Hash(model.confirmpassword);
                    try
                    {
                        AppCore.Repositories.UserRepository obj = new AppCore.Repositories.UserRepository();
                        AppCore.Models.User obj2 = obj.Get(model.userModel.ID);
                        obj2.Password = model.userModel.Password;
                        obj.Save(obj2);

                        ModelState.AddModelError("error_msg", "Reset password sucessfully");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("error_msg", "Có lỗi xảy ra");
                    }
                }
            }
            else
            {
                return Redirect("/Home/Index");
            }
            return View(model);
        }
        #endregion

        #region Xử lý công việc : danh sách, thêm, block
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            AccountModels modelSearch = new AccountModels();
            modelSearch.length = 10;
            modelSearch.RowIndex = 1;
            modelSearch.StringSearch = "";
            int totalRow = 0;
            //modelSearch.ListUser = UserCache.Getlist(modelSearch.StringSearch, out totalRow, modelSearch.length, modelSearch.RowIndex);
            modelSearch.ListUser = UserCache.Getlist();
            modelSearch.total = totalRow;

            return View(modelSearch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult block(int id)
        {
            AppCore.Repositories.UserRepository obj = new AppCore.Repositories.UserRepository();
            AppCore.Models.User _user = obj.Get(id);
            if (_user.Status == (int)AppExtension.StatusCommon.Approved)
            {
                _user.Status = (int)AppExtension.StatusCommon.Locked;
            }
            else
            {
                _user.Status = (int)AppExtension.StatusCommon.Approved;
            }
            obj.Save(_user);

            var result = new
            {
                Status = _user.Status,
                Success = true,
                Message = ""
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var model = new AccountModels();

            #region setup model
            model.userModel = new AppCacheEngines.EntitiesClass.User();
            model.strDVHC = "1";
            model.strRole = "";

            var dataOption = UserRoleCache.Getlist().Select(d => new { id = d.ID, name = d.RoleName });
            model.selectUserRole  = new SelectList(dataOption, "id", "name", model.strRole);
    
            
            var dataOptionDVHC = DVHCCache.Getlist().Select(d => new { id = d.IDDVHC, name = d.TenDVHC });
            model.selectDVHC = new SelectList(dataOptionDVHC, "id", "name", model.strDVHC);
            
            #endregion
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(AccountModels model)
        {
            UserRepository OBJ = new UserRepository();
            AppCore.Models.User _user = new AppCore.Models.User();
            var user = UserCache.GetCheckexists(model.userModel.UserName);
            if (user == null)
            {

                _user.Status = (int)StatusCommon.Approved;
                _user.Password = Md5Manage.GetMd5Hash(model.userModel.Password);
                _user.Email = model.userModel.Email;
                _user.Account = model.userModel.UserName;
                _user.IDDVHC = int.Parse(model.strDVHC);
                _user.IDRole = int.Parse(model.strRole);
                _user.Modified = DateTime.Now;
                int statuscreate = OBJ.Create(_user);
                if (statuscreate > 0)
                {
                    model.strMessage = "Đã tạo tài khoản thành công!";                 
                }
                else
                {
                    model.strMessage = "Có lỗi xảy ra!";
                }
            }
            else {
                model.strMessage = "Tài khoản này đã có trong hệ thống!";
            }

            #region setup model

            model.ListUserRole = UserRoleCache.Getlist();
            var dataOption = model.ListUserRole.Select(d => new { id = d.ID, name = d.RoleName });
            model.selectUserRole = new SelectList(dataOption, "id", "name", model.strRole);

            model.ListDVHC = DVHCCache.Getlist();
            var dataOptionDVHC = model.ListDVHC.Select(d => new { id = d.IDDVHC, name = d.TenDVHC });
            model.selectDVHC = new SelectList(dataOptionDVHC, "id", "name", model.strDVHC);
            model.strDVHC = _user.IDDVHC + "";
            model.strRole = _user.IDRole + "";

            #endregion
            return View(model);
        }

        #endregion

    }
}
