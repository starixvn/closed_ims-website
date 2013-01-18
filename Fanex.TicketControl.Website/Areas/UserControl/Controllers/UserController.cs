using System;
using System.Web.Mvc;
using Fanex.NAP.Core.Service.Interface;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.Criteria.User;
using Fanex.Logging;
using Fanex.Common;
using Fanex.NAP.Core.Web;
using Fanex.TicketControl.Core.Web;
using Fanex.TicketControl.Core.Web.Controllers;

namespace Fanex.TicketControl.Website.Areas.UserControl.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /UserControl/User/
        private readonly IUserService _userService;

        public UserController()
            : this(null)
        {
        }

        public UserController(IUserService userService)
        {
            _userService = userService ?? new UserService
                {
                    ServiceContext = ServiceContext
                };
        }

        #region Change Password

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new UpdatePasswordCustCriteria { Username = AppSession.CurrentUser.Name });
        }

        [HttpPost]
        public ActionResult ChangePassword(UpdatePasswordCustCriteria criteria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userService.GetById(AppSession.CurrentUser.Id);
                    string oldPwd = Cryptography.CfsEnCode(criteria.OldUserPwd);

                    if (oldPwd.Equals(user.Password))
                    {
                        criteria.CustId = user.Id;
                        criteria.UserPwd = Cryptography.CfsEnCode(criteria.UserPwd);

                        var result = _userService.InsertOrUpdate(criteria);

                        if (result != null && result.IsSuccessful)
                        {
                            ViewBag.Success = true;
                        }
                        else if (result != null && result.IsSuccessful == false)
                        {
                            ViewBag.Message = result.Message;
                        }
                    }
                    else
                    {
                        ViewBag.Message = ViewResources.Wrong_password;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        #endregion

        #region Profile

        [HttpGet]
        public ActionResult UserProfile()
        {
            var criteria = new UpdateCustCriteria();

            try
            {
                var user = _userService.GetById(AppSession.CurrentUser.Id);

                criteria.CustId = user.Id;
                criteria.Username = user.Username;
                criteria.FirstName = user.FirstName;
                criteria.LastName = user.LastName;
                criteria.Email = user.Email;
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        public ActionResult UserProfile(UpdateCustCriteria criteria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    criteria.CustId = AppSession.CurrentUser.Id;
                    criteria.Username = AppSession.CurrentUser.Username;
                    criteria.FirstName = criteria.FirstName ?? string.Empty;
                    criteria.LastName = criteria.LastName ?? string.Empty;
                    criteria.Email = criteria.Email ?? string.Empty;

                    var result = _userService.InsertOrUpdate(criteria);

                    if (result != null && result.IsSuccessful)
                    {
                        ViewBag.Success = true;
                    }
                    else if (result != null && result.IsSuccessful == false)
                    {
                        ViewBag.Message = result.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        #endregion
    }
}
