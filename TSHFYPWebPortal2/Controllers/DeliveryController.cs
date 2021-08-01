using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TSHFYPWebPortal2.Models;

namespace TSHFYPWebPortal2.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly DatabaseContext _context;
        public DeliveryController(DatabaseContext context)
        {
            _context = context;
        }




        //---------------------------------------------------------------------//
        //Upload delivery order. View: Upload.cshtml
        //For suppliers only
        public IActionResult Upload()
        {
            return View();
        }
      

        [HttpPost]
        public IActionResult Upload(IFormFile files)
        {
            if (files != null)
            {
                if (files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    if (fileExtension.Equals(".pdf")) //to allow only pdf files to be uploaded
                    {




                        var objfiles = new Delivery()
                        {
                            DocumentId = 0,
                            Name = User.Identity.Name + " " + newFileName,
                            FileType = fileExtension,
                            CreatedOn = DateTime.Now


                        };

                        using (var target = new MemoryStream())
                        {
                            files.CopyTo(target);
                            objfiles.DataFiles = target.ToArray();
                        }

                        _context.files.Add(objfiles);
                        _context.SaveChanges();
                        TempData["Message"] = "Delivery Order Uploaded";
                        TempData["MsgType"] = "success";



                    }
                    else
                    {
                        TempData["Message"] = "Invalid Format";
                        TempData["MsgType"] = "danger";
                        return View("upload");
                    }

                }



            }
            

            return RedirectToAction("Portal", "Order");
        }








        //---------------------------------------------------------------------//
        //To display all uploaded delivery orders. View: Table.cshtml
        //Only Warehouse, purchaser and admin user can view delivery order

        public IActionResult Table()
        {

            DataTable dt = DBUtl.GetTable("SELECT * FROM DeliveryOrder");


           



            return View("Table", dt.Rows);

        }












        //---------------------------------------------------------------------//
        //To display a specific delivery order pdf. View: ViewDO.cshtml
        //Only Warehouse and admin user can view delivery order

        public IActionResult ViewDO(int id)
        {
            
            string updatesql = @"SELECT * FROM DeliveryOrder
                               WHERE DocumentId = {0}";
            List<Delivery> lstorder = DBUtl.GetList<Delivery>(updatesql, id);
            var PdfFile = _context.files.Where(p => p.DocumentId == id).Select(s => s.DataFiles).FirstOrDefault();
            ViewData["PDF"] = PdfFile;
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
        //---------------------------------------------------------------------//
        // Method to delete delivery order. View: Table.cshtml
        // Only admin can delete
        public IActionResult Delete(int id)
        {
            string select = @"SELECT * FROM DeliveryOrder WHERE DocumentId={0}";
            DataTable ds = DBUtl.GetTable(select, id);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Order does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                string delete = "DELETE FROM DeliveryOrder WHERE DocumentId={0}";
                int res = DBUtl.ExecSQL(delete, id);
                if (res == 1)
                {
                    TempData["Message"] = "Delivery Order Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("table");
        }




    }
}
