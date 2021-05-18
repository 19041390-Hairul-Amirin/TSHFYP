using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSHFYPWebPortal2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

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



    
  


}
}