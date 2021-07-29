using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSHFYPWebPortal2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;

namespace TSHFYPWebPortal2.Controllers
{
    public class OrderController : Controller
    {

       



        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public OrderController(IHostingEnvironment environment, IWebHostEnvironment webHostEnvironment)
        {
            hostingEnvironment = environment;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public IActionResult UploadDO(UploadDO model)
        {

            if (ModelState.IsValid)
            {




                // do other validations on your model as needed
                if (model.File != null)
                {
                    var uniqueFileName = GetUniqueFileName(model.File.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    model.File.CopyTo(new FileStream(filePath, FileMode.Create));





                    TempData["Message"] = "Delivery Order Uploaded";
                    TempData["MsgType"] = "success";
                }
            }

                // to do  : Return something
                return RedirectToAction("Portal");
            }
            private string GetUniqueFileName(string fileName)
            {
            
                String username = User.Identity.Name;

                fileName = Path.GetFileName(fileName);
                return Path.GetFileNameWithoutExtension(fileName)
                          + "_"
                          + username + "_"
                          + Guid.NewGuid().ToString().Substring(0, 1)
                          + Path.GetExtension(fileName);
            }


        



            public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult UploadDO()
        {
            return View();
        }




        [HttpPost]


  


        //Different views for each supplier
        //They can only view 

        [HttpGet]
        [Authorize]
        public IActionResult Portal()
        {
            if (User.IsInRole("Supplier"))
            {
                string username = User.Identity.Name;
                DataTable dt = DBUtl.GetTable($"SELECT * FROM PurchaseOrder1 WHERE SupplierName='{username}'");
                return View("Supplier", dt.Rows); ;
            }
            else if (User.IsInRole("admin"))
            {
                DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder1");
                return View("Admin", dt.Rows); ;
            }
            else if (User.IsInRole("purchaser"))
            {
                DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder1");
                return View("Purchaser", dt.Rows); ;
            }
            else if (User.IsInRole("Warehouse"))
            {
                DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder1");
                return View("Warehouse", dt.Rows); ;
            }
            else if (User.IsInRole("account"))
            {
                DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder1");
                return View("Account", dt.Rows); ;
            }
            else if (User.IsInRole("manager"))
            {
                DataTable dt = DBUtl.GetTable("SELECT * FROM PurchaseOrder1");
                return View("SCM", dt.Rows); ;
            }
            else
            {
                
                return View("About"); ;
            }
        }


        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("About");
        }

        public ActionResult Cancel()
        {
            TempData["Message"] = "Rejected";
            TempData["MsgType"] = "error";
            return View("Portal");
        }



        [HttpGet]
        public IActionResult Add()
        {
         return View("Add");
            
        }

        public IActionResult Add(Order ord)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Add");
            }
            else
            {
                string insert =
                    @"INSERT INTO PurchaseOrder1(OrderDate, DueDate, RevisedDate, PONum, PRNum, SupplierID, SupplierName, Payment, PartNum, Descr, JobNum, Currency, QTY, UOM, UnitPrice, AMT, TSHCMPONum, Request, Purchaser) VALUES
                      ('{0:yyyy-MM-dd}','{1:yyyy-MM-dd}','{2:yyyy-MM-dd}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}')  ";
                int result = DBUtl.ExecSQL(insert, ord.OrderDate, ord.DueDate, ord.RevisedDate, ord.PONum, ord.PRNum, ord.SupplierID, ord.SupplierName, ord.Payment, ord.PartNum, ord.Descr, ord.JobNum, ord.Currency, ord.Qty, ord.UOM, ord.UnitPrice, ord.AMT, ord.TSHCMPONUm, ord.Request, ord.Purchaser);


                if (result == 1)
                {
                    TempData["Message"] = "Purchase Order created";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("Portal");
            }
        }


        public IActionResult Delete(int id)
        {
            string select = @"SELECT * FROM PurchaseOrder1 WHERE PId={0}";
            DataTable ds = DBUtl.GetTable(select, id);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Order does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                string delete = "DELETE FROM PurchaseOrder1 WHERE PId={0}";
                int res = DBUtl.ExecSQL(delete, id);
                if (res == 1)
                {
                    TempData["Message"] = "Order Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("Portal");
        }

        [HttpGet]
        public IActionResult Update(int id)
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
                return RedirectToAction("Supplier");
            }

        }

        [HttpPost]
        public IActionResult Update(Order ord)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Update");
            }
            else 
            {
                string edit =
                   @"UPDATE PurchaseOrder1
                    SET PONum='{1}', Descr='{2}',OrderDate='{3:yyyy-MM-dd}',RevisedDate='{4:yyyy-MM-dd}', SupplierName='{5}', Status='{6}' WHERE PId={0}";
                int res = DBUtl.ExecSQL(edit, ord.PId, ord.PONum, ord.Descr, ord.OrderDate, ord.RevisedDate, ord.SupplierName, ord.Status);
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
                return RedirectToAction("Portal");
            }

        }

        [HttpGet]
        public IActionResult Accept(int id)
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
                return RedirectToAction("Portal","Order");
            }

        }


        
        [HttpPost]
        public IActionResult Accept(Order ord)
        {
            string username = User.Identity.Name;
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Accept");
            }
           
            else if (ord.SupplierName.Equals(username))
            {
                string edit =
                   @"UPDATE PurchaseOrder1
                    SET PONum='{1}', Descr='{2}',OrderDate='{3:yyyy-MM-dd}',RevisedDate='{4:yyyy-MM-dd}', SupplierName='{5}', Status ='Accepted' WHERE PId={0}";
                int res = DBUtl.ExecSQL(edit, ord.PId, ord.PONum, ord.Descr, ord.OrderDate, ord.RevisedDate, ord.SupplierName);

                if (res == 1)
                {

                    List<TSHUsers> list = DBUtl.GetList<TSHUsers>("SELECT * FROM TSHUsers");
                    foreach (TSHUsers users in list)
                    {

                        if (username.Equals(users.UserId))
                        {
                            try
                            {
                                // Credentials
                                var credentials = new NetworkCredential("FeedbackTSH@gmail.com", "FYPTsH!1");

                                // Mail message to clients
                                var mail = new MailMessage()
                                {
                                    From = new MailAddress("FeedbackTSH@gmail.com"),
                                    Subject = "DoNotReply-FeedbackTSH@gmail.com",
                                    Body = "PURCHASE ORDER ACCEPTED SUCCESSFULLY </br> " +
                                    "This is an email of acknowledgement to confirmed that you have accepted the Purchase Order </br>" +
                                    " Thank You for your response!"
                                };


                                // Mail message to TSH live NOT DONE
                                var mail2Me = new MailMessage()
                                {
                                    From = new MailAddress(users.Email),
                                    Subject = users.FullName + ", Purchase Order Acknowledgement",
                                    Body = users.UserId + " have accepted the Order Confirmation"
                                };

                                mail.IsBodyHtml = true;
                                mail.To.Add(new MailAddress(users.Email));

                                mail2Me.IsBodyHtml = true;
                                mail2Me.To.Add(new MailAddress("FeedbackTSH@gmail.com"));

                                // Smtp client
                                var client = new SmtpClient()
                                {
                                    Port = 587,
                                    DeliveryMethod = SmtpDeliveryMethod.Network,
                                    UseDefaultCredentials = false,
                                    Host = "smtp.gmail.com",
                                    EnableSsl = true,
                                    Credentials = credentials
                                };

                                client.Send(mail);
                                client.Send(mail2Me);

                                //return "Email Sent Successfully!";
                            }
                            catch (System.Exception e)
                            {
                                //return e.Message;
                            }

                            return RedirectToAction("Portal", "Order");
                        }
                    }


                    TempData["Message"] = "Order Accepted";
                    TempData["MsgType"] = "success";
                    ord.Accepted = "Accepted";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("Portal","Order");
            }

            return RedirectToAction("Portal", "Order");





          
        }















        [HttpGet]
        public IActionResult ViewOrder(int id)
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
                return RedirectToAction("Portal", "Order");
            }

        }



        [HttpPost]
        public IActionResult ViewOrder(Order ord)
        {
            string username = User.Identity.Name;
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Accept");
            }

            else if (ord.SupplierName.Equals(username))
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
                return RedirectToAction("Portal", "Order");
            }

            return RedirectToAction("Portal", "Order");






        }















        public JsonResult OpenPDFPath()
        {
            string PDFpath = "uploads/pdf/report_GTI_d.PDF";
            return Json(PDFpath);
        }


        public FileResult OpenPDF()
        {
            string PDFpath = "wwwroot/uploads/report_GTI_d.PDF";
            byte[] abc = System.IO.File.ReadAllBytes(PDFpath);
            System.IO.File.WriteAllBytes(PDFpath, abc);
            MemoryStream ms = new MemoryStream(abc);
            return new FileStreamResult(ms, "application/pdf");
        }


       






    }
}