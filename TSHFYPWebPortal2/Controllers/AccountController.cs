using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using TSHFYPWebPortal2.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace TSHFYPWebPortal2.Controllers
{
    public class AccountController : Controller
    {
        private const string LOGIN_SQL =
           @"SELECT * FROM TSHUsers 
            WHERE UserId = '{0}' 
              AND UserPw = HASHBYTES('SHA1', '{1}')";

        private const string LASTLOGIN_SQL =
           @"UPDATE TSHUsers SET LastLogin=GETDATE() WHERE UserId='{0}'";

        private const string ROLE_COL = "UserRole";
        private const string NAME_COL = "FullName";

        private const string REDIRECT_CNTR = "Order";
        private const string REDIRECT_ACTN = "Index";

        private const string LOGIN_VIEW = "UserLogin";

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(LOGIN_VIEW);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLogin user, TSHUsers name)
        {
            if (!AuthenticateUser(user.UserID, user.Password, out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect User ID or Password";
                ViewData["MsgType"] = "warning";
                return View(LOGIN_VIEW);
            }
            else
            {
                HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal);
                string select = $"SELECT * FROM TSHUsers WHERE UserId='{user.UserID}'";
                if (DBUtl.GetTable(select).Rows.Count > 0)
                {
                    if (TempData["returnUrl"] != null)
                    {
                        string returnUrl = TempData["returnUrl"].ToString();
                        if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);

                    }

                    return RedirectToAction("Portal", "Order"); 
                }
                return View();

               
            }
        }

        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
        }
        /*
                [AllowAnonymous]
                public IActionResult Forbidden()
                {
                    return View();
                }

                [Authorize(Roles = "manager")]
                public IActionResult Users()
                {
                    List<TSHUsers> list = DBUtl.GetList<TSHUsers>("SELECT * FROM TSHUsers WHERE UserRole='member' ");
                    return View(list);
                }
        */

        public IActionResult UserList()
        {
            DataTable dt = DBUtl.GetTable("SELECT * FROM TSHUsers");
            return View("UserList", dt.Rows);
        }

        #region "User Registration"
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("UserRegister");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(TSHUsers usr)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("UserRegister");
            }
            else
            {
                string insert =
                   @"INSERT INTO TSHUsers(UserId, UserPw, FullName, Email, UserRole) 
                        VALUES('{0}', HASHBYTES('SHA1','{1}'), '{2}', '{3}', '{4}')";
                if (DBUtl.ExecSQL(insert, usr.UserId, usr.UserPw, usr.FullName, usr.Email, usr.UserRole) == 1)
                {
                    ViewData["Message"] = "User Successfully Registered";
                    ViewData["MsgType"] = "success";
                    return RedirectToAction("Portal", "Order");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                    return View("UserRegister");
                }
            }
        }
        #endregion

        #region "DeleteUser"
        public IActionResult DeleteUser(string id)
        {
            string delete = "DELETE FROM TSHUsers WHERE UserId='{0}'";
            int res = DBUtl.ExecSQL(delete, id);
            if (res == 1)
            {
                TempData["Message"] = "User Record Deleted";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }

            return RedirectToAction("UserList");
        }
        #endregion

        #region "Update User"
        [HttpGet]
        public IActionResult UpdateUserList(string id)
        {
            string updatesql = @"SELECT * FROM TSHUsers
                                WHERE UserId = '{0}'";
            List<TSHUsers> user = DBUtl.GetList<TSHUsers>(updatesql, id);
            if (user.Count == 1)
            {
                return View(user[0]);
            }
            else
            {
                TempData["Message"] = "Account not found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("UserList");
            }
        }

        [HttpPost]
        public IActionResult UpdateUserList(TSHUsers usr)
        {
            ModelState.Remove("UserPw2");

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("UpdateUserList");
            }
            else
            {
                string edit =
                   @"UPDATE TSHUsers
                    SET userPw=HASHBYTES('SHA1','{1}'), FullName='{2}',Email='{3}',UserRole='{4}' WHERE UserId='{0}'";

                if (DBUtl.ExecSQL(edit,usr.UserId, usr.UserPw, usr.FullName, usr.Email, usr.UserRole) == 1)
                {
                    TempData["Message"] = "User Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("UserList");
            }
        }
        #endregion

        [AllowAnonymous]
        public IActionResult VerifyUserID(string userId)
        {
            string select = $"SELECT * FROM TSHUsers WHERE Userid='{userId}'";
            if (DBUtl.GetTable(select).Rows.Count > 0)
            {
                return Json($"[{userId}] already in use");
            }
            return Json(true);
        }

        private bool AuthenticateUser(string uid, string pw, out ClaimsPrincipal principal)
        {
            principal = null;

            DataTable ds = DBUtl.GetTable(LOGIN_SQL, uid, pw);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, uid),
                        new Claim(ClaimTypes.Name, ds.Rows[0][NAME_COL].ToString()),
                        new Claim(ClaimTypes.Role, ds.Rows[0][ROLE_COL].ToString())
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }

    }
}