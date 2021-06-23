using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace TSHFYPWebPortal2.Models
{
   public class Order
   {
        public int PId { get; set; }
        public int sts { get; set; }
        public int Bflag { get; set; }



        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime RevisedDate { get; set; }
        public string PONum { get; set; }
        public string PRNum { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Descr { get; set; }
        public int Qty { get; set; }
        public string Accepted { get; set; }
        public string Payment { get; set; }
        public string PartNum { get; set; }
        
    }

}
