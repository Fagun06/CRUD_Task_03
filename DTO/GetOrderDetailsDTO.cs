namespace CRUD_Task_03.DTO
{
    public class GetOrderDetailsDTO
    {
        public GetOrderDetailsHeaderDTO getOrderDetailsHeader { get; set; }

        public List<GetOrderDetailsRowDTO> Rows { get; set; }
    }

    public class GetCustomerDetailsWithItemDTO
    {
        public GetOrderDetailsHeaderDTO getOrderDetailsHeader { get; set; }

    }
}
