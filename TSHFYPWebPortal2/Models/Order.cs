using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace TSHFYPWebPortal2.Models
{
   public class Order
   {
        public int PId { get; set; }  
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime RevisedDate { get; set; }
        public string PONum { get; set; }
        public string PRNum { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Payment { get; set; }
        public string PartNum { get; set; }
        public string Descr { get; set; }
        public string JobNum { get; set; }
        public string Currency { get; set; }
        public float Qty { get; set; }
        public string UOM { get; set; }
        public float UnitPrice { get; set; }
        public float AMT { get; set; }
        public string TSHCMPONUm { get; set; }
        public string Request { get; set; }
        public string Purchaser { get; set; }
        public string Status { get; set; }
        public string Accepted { get; set; }
        public string DO { get; set; }








    }

   

    }
