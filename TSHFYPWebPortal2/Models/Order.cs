using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace TSHFYPWebPortal2.Models
{
   public class Order
   {
        public int Pid { get; set; }
        public int sts { get; set; }
        public int Bflag { get; set; }



        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime RevDelDate { get; set; }
        public string POnum { get; set; }
        public string PRnum { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
    }

}
