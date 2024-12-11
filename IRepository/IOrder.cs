using CRUD_Task_03.DTO;
using CRUD_Task_03.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Task_03.IRepository
{
    public interface IOrder
    {
        Task<MessageHelper> CreateOrderWithItem(CreateOrderDTO create);
        Task<MessageHelper> UpdateOrderWithItem(UpdateOrderDTO update);
        Task<GetOrderDetailsDTO> GetOrderDetails(int Id);
        Task<MessageHelper> DeleteOrder(int Id);
        Task<MaxAndMinDTO> MinimumAndMaximum();
        Task<MinMaxDTO> MaxAndMin();
        Task<List<GetOrderDetailsHeaderDTO>> SearchByCustormerName(string name);
        Task<List<GetOrderDetailsHeaderDTO>> DateRang(DateTime fromDate, DateTime ToDate);
        Task<DateRangTotalAmountDTO> DateRangTotalAmount(DateTime fromDate, DateTime ToDate);
        Task<List<DailyTotalSalesDTO>> DailyTotalSales();
        Task<MessageHelper> CreateOrdersWithItemBulkInsert(List<CreateOrderDTO> createOrders);
        Task<List<OrderListDTO>> OrdersListBypagination(int pageNo, int PageSize);
        Task<List<GetOrderDetailsHeaderrDTO>> FiltersByDateTimeMinMaxCustormerName(OrderFilterDTO filter);
        Task<List<DailyTotalSalesDTO>> GetByDateTimeTotalAmountRang(DateTime fromDate, DateTime ToDate);
    }   
}
