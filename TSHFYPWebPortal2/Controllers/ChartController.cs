using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using TSHFYPWebPortal2.Models;

namespace TSHFYPWebPortal2.Controllers
{
    public class ChartController : Controller
    {

       
        
        public IActionResult Index()
        {
            return View();
        }

        

    


    public IActionResult Test()
        {
            return View("Test");


         }
        public IActionResult Name()
        {
            PrepareData(1); 
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Supplier Name";
            ViewData["ShowLegend"] = "true";
            return View("Summary");
        }



        public IActionResult Fruit()
        {
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Supplier Name";
            ViewData["ShowLegend"] = "true";
            return View("Summary");


        }

            public IActionResult Value()
        {
            PrepareData(2);
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Top 10 Supplier by Value";
            ViewData["ShowLegend"] = "true";
            return View("Summary");
        }
        [HttpGet]
        private void PrepareData(int x)
        {
            
            int[] name = new int[] { 0, 0, 0, 0, 0, 0 };
            float[] price = new float[] { 0f, 0f, 0f, 0f, 0f, 0f };
            List<Order> list = DBUtl.GetList<Order>("SELECT * FROM PurchaseOrder1");
            foreach (Order order in list)
            {
                
                if (order.SupplierName.Equals("GTI")) name[0]++;
                else if (order.SupplierName.Equals("IFME")) name[1]++;
                else if (order.SupplierName.Equals("KHS")) name[2]++;
                else if (order.SupplierName.Equals("KSPAI")) name[3]++;
                else if (order.SupplierName.Equals("PPP")) name[4]++;
                else if (order.SupplierName.Equals("TEI")) name[5]++;




                if (order.SupplierName.Equals("GTI"))
                {
                    
                    price[0] += order.UnitPrice;
                }
                else if (order.SupplierName.Equals("IFME")) price[1] = order.UnitPrice;
                else if (order.SupplierName.Equals("KSPAI")) price[2] = order.UnitPrice;
                else if (order.SupplierName.Equals("KHS")) price[3] = order.UnitPrice;
                else if (order.SupplierName.Equals("PPP")) price[4] = order.UnitPrice;
                else if (order.SupplierName.Equals("TEI")) price[5] = order.UnitPrice;


             

            }

            // Supplier Value
            
            
          

            if (x == 1)
            {
                Array.Sort(name);
                ViewData["Legend"] = "Orders by Suppliers Name";
                ViewData["Colors"] = new[] { "violet", "green", "blue", "orange", "red","yellow" };
                ViewData["Labels"] = new[] { "GTI", "IFME", "KHS", "KSPAI", "PPP","TEI" };
                ViewData["Data"] = name;
            }
            //Top 10 Supplier by Value
            else if (x == 2)
            {
                ViewData["Legend"] = "Top 10 Suppliers by Value ($)";
                ViewData["Colors"] = new[] { "red", "green", "orange", "blue", "yellow", "indigo" };
                ViewData["Labels"] = new[] { "GTI", "IFME", "KHS", "KSPAI", "PPP", "TEI" };
                ViewData["Data"] = price;
            }
            else
            {
                ViewData["Legend"] = "Nothing";
                ViewData["Colors"] = new[] { "gray", "darkgray", "black" };
                ViewData["Labels"] = new[] { "X", "Y", "Z" };
                ViewData["Data"] = new int[] { 1, 2, 3 };
            }

        }


      


    }
}
//new string[] { "GTI", "IFME", "KHS", "KSPAI", "PPP", "TEI" };