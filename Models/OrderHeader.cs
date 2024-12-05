using System;
using System.Collections.Generic;

namespace CRUD_Task_03.Models
{
    public partial class OrderHeader
    {
        public long OrderId { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool? IsActive { get; set; }
    }
}
