namespace CRUD_Task_03.DTO
{
    public class UpdateOrderDTO
    {
        public UpdateOrderHeadDTO UpdateOrderHead { get; set; }
        public List<UpdateOrderRowDTO> Rows { get; set; }
     }
}
