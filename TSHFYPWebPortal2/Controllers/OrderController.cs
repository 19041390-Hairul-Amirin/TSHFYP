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

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact Us";

            return View();
        }


public IActionResult Index()
    {

            DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder1");
            return View("Index", dt.Rows);
            
       
           

    }


        //Different views for each supplier
        //They can only view 
        public IActionResult GTI()
        {
            DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder1 WHERE SupplierName='GTI'");
            return View("Index", dt.Rows); ;
        }

        public IActionResult IFME()
        {
            DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder1 WHERE SupplierName='IFME'");
            return View("Index", dt.Rows); ;
        }

        public IActionResult KHS()
        {
            DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder1 WHERE SupplierName='KHS'");
            return View("Index", dt.Rows); ;
        }
        private const string REDIRECT_CNTR = "Order";
        private const string REDIRECT_ACTN = "Index";

        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("About");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            string updatesql = @"SELECT * FROM PurchaseOrder1
                               WHERE PId = {0}";
            List<Order> lstorder = DBUtl.GetList<Order>(updatesql, id);
            if (lstorder.Count == 1)
            {
                return View(lstorder[0]);
            }
            else
            {
                TempData["Message"] = "Order not found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }

        }


        public ActionResult Cancel()
        {
            TempData["Message"] = "Rejected";
            TempData["MsgType"] = "error";
            return View("Index");
        }
        [HttpPost]
        public IActionResult Edit(Order ord)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Edit");
            }


            //Accept function for each supplier 






            //GTI
            else if (ord.SupplierName.Equals("GTI"))
            {
                string edit =
                   @"UPDATE PurchaseOrder1
                    SET PONum='{1}', Descr='{2}',OrderDate='{3:yyyy-MM-dd}',RevisedDate='{4:yyyy-MM-dd}', SupplierName='{5}', Status ='Accepted' WHERE PId={0}";
                int res = DBUtl.ExecSQL(edit, ord.PId, ord.PONum, ord.Descr, ord.OrderDate, ord.RevisedDate, ord.SupplierName);
                if (res == 1)
                {
                    TempData["Message"] = "Order Updated";
                    TempData["MsgType"] = "success";

                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("GTI");
            }







            //IFME
            else if (ord.SupplierName.Equals("IFME"))
            {
                string edit =
                   @"UPDATE PurchaseOrder1
                    SET PONum='{1}', Descr='{2}',OrderDate='{3:yyyy-MM-dd}',RevisedDate='{4:yyyy-MM-dd}', SupplierName='{5}', Status ='Accepted' WHERE PId={0}";
                int res = DBUtl.ExecSQL(edit, ord.PId, ord.PONum, ord.Descr, ord.OrderDate, ord.RevisedDate, ord.SupplierName);
                if (res == 1)
                {
                    TempData["Message"] = "Order Accepted";
                    TempData["MsgType"] = "success";
                   

                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("IFME");
            }









            //KHS
            else if (ord.SupplierName.Equals("KHS"))
            {
                string edit =
                   @"UPDATE PurchaseOrder1
                    SET PONum='{1}', Descr='{2}',OrderDate='{3:yyyy-MM-dd}',RevisedDate='{4:yyyy-MM-dd}', SupplierName='{5}', Status ='Accepted' WHERE PId={0}";
                int res = DBUtl.ExecSQL(edit, ord.PId, ord.PONum, ord.Descr, ord.OrderDate, ord.RevisedDate, ord.SupplierName);
                if (res == 1)
                {
                    TempData["Message"] = "Order Accepted";
                    TempData["MsgType"] = "success";
                    ord.accepted = "Accepted";

                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("KHS");
            }

            else 
            {
                string edit =
                  @"UPDATE PurchaseOrder1
                    SET PONum='{1}', Descr='{2}',OrderDate='{3:yyyy-MM-dd}',RevisedDate='{4:yyyy-MM-dd}', SupplierName='{5}' WHERE PId={0}";
                int res = DBUtl.ExecSQL(edit, ord.PId, ord.PONum, ord.Descr, ord.OrderDate, ord.RevisedDate, ord.SupplierName);
                if (res == 1)
                {
                    TempData["Message"] = "Order Accepted";
                    TempData["MsgType"] = "success";
                    ord.accepted = "Accepted";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("Index");
            }
        }



    }
}