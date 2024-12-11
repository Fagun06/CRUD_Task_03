namespace CRUD_Task_03.DTO
{
    public class GetOrderDetailsRowwDTO
    {
        public long OrderItemId { get; set; }

        public long OrderId { get; set; }
        public string ProductName { get; set; }
        public long Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
