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

                    var objfiles = new Delivery()
                    {
                        DocumentId = 0,
                        Name = newFileName,
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

                }
            }
            return View("Upload");
        }

        public IActionResult Table()
        {

            DataTable dt = DBUtl.GetTable("SELECT * FROM DeliveryOrder");


           



            return View("Table", dt.Rows);

        }

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




    }
}
