namespace CRUD_Task_03.DTO
{
    public class OrderFilterDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinTotalAmount { get; set; }
        public decimal? MaxTotalAmount { get; set; }
        public string? CustomerName { get; set; }
    }
}
