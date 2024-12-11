using System;
using System.Collections.Generic;

namespace CRUD_Task_03.Models
{
    public partial class OrderRow
    {
        public long OrderItemId { get; set; }
        public long OrderId { get; set; }
        public string ProductName { get; set; } = null!;
        public long Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool? IsActive { get; set; }
    }
}
