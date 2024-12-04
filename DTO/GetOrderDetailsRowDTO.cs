namespace CRUD_Task_03.DTO
{
    public class GetOrderDetailsRowDTO
    {
        public long OrderItemId { get; set; }
        public string ProductName { get; set; }
        public long Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
