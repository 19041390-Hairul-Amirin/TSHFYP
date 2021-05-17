using System;

namespace TSHFYPWebPortal.Models
{
    public class Orders
    {
        public int Pid { get; }
        public int sts { get; }
        public int Bflag { get; }

        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime RevDelDate { get; set; }
        public string POnum { get; set; }
        public string PRnum { get; set; }
        public string SupplierID { get; set; }
    }
}
