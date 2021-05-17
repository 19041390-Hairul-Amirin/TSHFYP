using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TSHFYPWebPortal.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TSHFYPWebPortal.Controllers
{
    public class OrdersController : Controller
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
        
     

    }
}
