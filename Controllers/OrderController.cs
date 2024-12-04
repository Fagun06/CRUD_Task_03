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

        public async Task<MessageHelper> CreateOrderWithItem(CreateOrderDTO create)
        {
            var result = await _IOrderRepo.CreateOrderWithItem(create);
            return result;
        }

        [HttpPost]
        [Route("UpdateOrderWithItem")]

        public async Task<MessageHelper> UpdateOrderWithItem(UpdateOrderDTO update)
        {
            var result = await _IOrderRepo.UpdateOrderWithItem(update);
            return result;
        }

        [HttpGet]
        [Route("GetOrderDatails")]

        public async Task<GetOrderDetailsDTO> GetOrderDetails(int Id)
        {
            var result = await _IOrderRepo.GetOrderDetails(Id);
            return result;
        }
        [HttpPut]
        [Route("DeleteOrder")]

        public async Task<MessageHelper> DeleteOrder(int Id)
        {
            var result = await _IOrderRepo.DeleteOrder(Id);
            return result;
        }

        [HttpGet]
        [Route("MinimumAndMaximum")]

        public async Task<MaxAndMinDTO>MinimumAndMaximum()
        {
            var result = await _IOrderRepo.MinimumAndMaximum();
            return result;
        }

    }
}
