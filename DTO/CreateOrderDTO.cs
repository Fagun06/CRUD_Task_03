namespace CRUD_Task_03.DTO
{
    public class CreateOrderDTO
    {
        public CreateOrderHeadDTO CreateOrderHead { get; set; }
        public List<CreateOrderRowDTO> Rows { get; set; }

    }
}
