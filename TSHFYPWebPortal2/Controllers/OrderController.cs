using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSHFYPWebPortal2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace TSHFYPWebPortal2.Controllers
{
public class OrderController : Controller
{
   
    public IActionResult About()
    {
        return View();
    }

    
    public IActionResult Index()
    {
        DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder");
        return View("Index", dt.Rows);
        
    }

        private const string REDIRECT_CNTR = "Order";
        private const string REDIRECT_ACTN = "Index";

        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index");
        }


        // testing 

    }
}