using CRUD_Task_03.DTO;
using CRUD_Task_03.Helper;
using CRUD_Task_03.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Task_03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _IOrderRepo;

        public OrderController(IOrder iOrderRepo)
        {
            _IOrderRepo = iOrderRepo;
        }

        [HttpPost]
        [Route("CreateOrderWithItem")]

        public async Task<IActionResult> CreateOrderWithItem(CreateOrderDTO create)
        {
            var result = await _IOrderRepo.CreateOrderWithItem(create);
            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateOrderWithItem")]

        public async Task<IActionResult> UpdateOrderWithItem(UpdateOrderDTO update)
        {
            var result = await _IOrderRepo.UpdateOrderWithItem(update);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetOrderDatails")]

        public async Task<IActionResult> GetOrderDetails(int Id)
        {
            var result = await _IOrderRepo.GetOrderDetails(Id);
            return Ok(result);
        }
        [HttpPut]
        [Route("DeleteOrder")]

        public async Task<IActionResult> DeleteOrder(int Id)
        {
            var result = await _IOrderRepo.DeleteOrder(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route("MinimumAndMaximum")]

        public async Task<IActionResult> MinimumAndMaximum()
        {
            var result = await _IOrderRepo.MinimumAndMaximum();
            return Ok(result);
        }

        [HttpGet]
        [Route("SearchByCustormerName")]
        public async Task<IActionResult> SearchByCustormerName(string name)
        {
            var results =await _IOrderRepo.SearchByCustormerName(name);
            return Ok(results);
        }
        [HttpGet]
        [Route("GetByDateTime")]

        public async Task<IActionResult> DateRang(DateTime fromDate, DateTime ToDate)
        {
            var results = await _IOrderRepo.DateRang(fromDate, ToDate);
            return Ok(results);
        }

        [HttpGet]
        [Route("GetByDateTimeTotalAmount")]

        public async Task<IActionResult> GetByDateTimeTotalAmount(DateTime fromDate, DateTime ToDate)
        {
            var results = await _IOrderRepo.DateRangTotalAmount(fromDate, ToDate);
            return Ok(results);
        }


        [HttpGet]
        [Route("GetDailyTotalSales")]

        public async Task<IActionResult> DailyTotalSales() 
        {
            var results = await _IOrderRepo.DailyTotalSales();
            return Ok(results);
        }

        [HttpPost]
        [Route("CreateOrdersWithItemBulkInsert")]
        public async Task<IActionResult> CreateOrdersWithItemBulkInsert(List<CreateOrderDTO> createOrders)
        {
            var results = await _IOrderRepo.CreateOrdersWithItemBulkInsert(createOrders);
            return Ok(results);
        }

        [HttpGet]
        [Route("OrderListByPagination")]

        public async Task<IActionResult> OrderListByPagination(int pageNo, int PageSize)
        {
            var result = await _IOrderRepo.OrdersListBypagination(pageNo,PageSize);
            return Ok(result);
        }

    }
}
