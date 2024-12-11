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

        // API 01 Add new Order With a Item
        [HttpPost]
        [Route("CreateOrderWithItem")]

        public async Task<IActionResult> CreateOrderWithItem(CreateOrderDTO create)
        {
            var result = await _IOrderRepo.CreateOrderWithItem(create);
            return Ok(result);
        }

        // API 02 Update new Order With a Item
        [HttpPost]
        [Route("UpdateOrderWithItem")]

        public async Task<IActionResult> UpdateOrderWithItem(UpdateOrderDTO update)
        {
            var result = await _IOrderRepo.UpdateOrderWithItem(update);
            return Ok(result);
        }

        // API 03 Get all Order Details
        [HttpGet]
        [Route("GetOrderDatails")]

        public async Task<IActionResult> GetOrderDetails(int Id)
        {
            var result = await _IOrderRepo.GetOrderDetails(Id);
            return Ok(result);
        }

        // API 04 DeleteOrder
        [HttpPut]
        [Route("DeleteOrder")]

        public async Task<IActionResult> DeleteOrder(int Id)
        {
            var result = await _IOrderRepo.DeleteOrder(Id);
            return Ok(result);
        }

        // API 05 Minimum and Maximum TotalOrder
        [HttpGet]
        [Route("MinimumAndMaximum")]

        public async Task<IActionResult> MinimumAndMaximum()
        {
            var result = await _IOrderRepo.MinimumAndMaximum();
            return Ok(result);
        }

        //API 06 Minimum and Maximum Details Total Order
        [HttpGet]
        [Route("MinimumMaximumDetails")]

        public async Task<IActionResult> MaxAndMin()
        {
            var result = await _IOrderRepo.MaxAndMin();
            return Ok(result);
        }

        // API 07 Search By Customer Name

        [HttpGet]
        [Route("SearchByCustormerName")]
        public async Task<IActionResult> SearchByCustormerName(string name)
        {
            var results =await _IOrderRepo.SearchByCustormerName(name);
            return Ok(results);
        }

        // API 08 Get By DateTime
        [HttpGet]
        [Route("GetByDateTime")]

        public async Task<IActionResult> DateRang(DateTime fromDate, DateTime ToDate)
        {
            var results = await _IOrderRepo.DateRang(fromDate, ToDate);
            return Ok(results);
        }
        //API 09 GetByDateTimeTotalAmount
        [HttpGet]
        [Route("GetByDateTimeTotalAmount")]

        public async Task<IActionResult> GetByDateTimeTotalAmount(DateTime fromDate, DateTime ToDate)
        {
            var results = await _IOrderRepo.DateRangTotalAmount(fromDate, ToDate);
            return Ok(results);
        }

        //Api 10 GetDailyTotalSales
        [HttpGet]
        [Route("GetDailyTotalSales")]

        public async Task<IActionResult> DailyTotalSales() 
        {
            var results = await _IOrderRepo.DailyTotalSales();
            return Ok(results);
        }
        //API 11 Create BulkInsert Orders WithItem
        [HttpPost]
        [Route("CreateOrdersWithItemBulkInsert")]
        public async Task<IActionResult> CreateOrdersWithItemBulkInsert(List<CreateOrderDTO> createOrders)
        {
            var results = await _IOrderRepo.CreateOrdersWithItemBulkInsert(createOrders);
            return Ok(results);
        }

        //API 12 OrderListByPagination
        [HttpGet]
        [Route("OrderListByPagination")]

        public async Task<IActionResult> OrderListByPagination(int pageNo, int PageSize)
        {
            var result = await _IOrderRepo.OrdersListBypagination(pageNo,PageSize);
            return Ok(result);
        }
        // API 13 Filtering by Date Time by Max and Min CustomerName
        [HttpGet]
        [Route("FiltersByDateTimeMinMaxCustormerName")]
        public async Task<IActionResult> FiltersByDateTimeMinMaxCustormerName([FromQuery]OrderFilterDTO filter)
        {
            var result = await _IOrderRepo.FiltersByDateTimeMinMaxCustormerName(filter);
            return Ok(result);
        }


        //API 14 Get By Date Time Total Amount Rang

        [HttpGet]
        [Route("GetByDateTimeTotalAmountRang")]
        public async Task<IActionResult> GetByDateTimeTotalAmountRang(DateTime fromDate, DateTime ToDate)
        {
            var results = await _IOrderRepo.GetByDateTimeTotalAmountRang(fromDate, ToDate);
            return Ok(results);
        }
    }
}
