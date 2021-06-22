using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using TSHFYPWebPortal2.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Web.Helpers;

namespace TSHFYPWebPortal2.Controllers
{
    public class EmailController : Controller
    {
        // get email
        public IActionResult SendEmail()
        {
            return View();
        }

            // post request
            [HttpPost]
            public IActionResult SendEmail(string useremail)
        {
            string subject = "Email of Acknowledgement";
            string body = "We have received your updated version, Thank You";

            WebMail.Send(useremail, subject, body, null, null, null, true, null, null, null, null, null, null);
            ViewBag.msg = "Email Sent Successfully";
            return View();
        }
    }
}