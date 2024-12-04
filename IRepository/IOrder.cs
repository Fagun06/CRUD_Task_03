using CRUD_Task_03.DTO;
using CRUD_Task_03.Helper;

namespace CRUD_Task_03.IRepository
{
    public interface IOrder
    {
        Task<MessageHelper> CreateOrderWithItem(CreateOrderDTO create);

        Task<MessageHelper> UpdateOrderWithItem(UpdateOrderDTO update);

        Task<GetOrderDetailsDTO> GetOrderDetails(int Id);

        Task<MessageHelper> DeleteOrder(int Id);
        Task<MaxAndMinDTO> MinimumAndMaximum();
    }   
}
