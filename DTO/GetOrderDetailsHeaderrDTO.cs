namespace CRUD_Task_03.DTO
{
    public class GetOrderDetailsHeaderrDTO
    {
        public long OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? OrderDate { get; set; }
        public List<GetOrderDetailsRowwDTO> Rows { get; set; }
    }
}
