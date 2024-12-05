namespace CRUD_Task_03.DTO
{
    public class OrderListDTO
    {
        public  string  CustomerName { get; set; }

        public DateTime? OrderDate {  get; set; }

        public decimal TotalAmount { get; set; }
    }
}
