using CRUD_Task_03.DBContext;
using CRUD_Task_03.DTO;
using CRUD_Task_03.Helper;
using CRUD_Task_03.IRepository;
using CRUD_Task_03.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRUD_Task_03.Repository
{
    public class Order : IOrder
    {
        private readonly AppDbContext _context;

        public Order(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MessageHelper> CreateOrderWithItem(CreateOrderDTO create)
        {
            try
            {
                var newOrderHead = new OrderHeader
                {
                  
                    CustomerName = create.CreateOrderHead.CustomerName,
                    OrderDate = DateTime.Now,
                    TotalAmount = create.Rows.Sum(x => x.Quantity * x.UnitPrice),
                    IsActive = true
                };
                await _context.AddAsync(newOrderHead);
                await _context.SaveChangesAsync();

                var newOrderRows = create.Rows.Select(x=> new OrderRow 
                {
                    OrderItemId = x.OrderItemId,
                    OrderId = newOrderHead.OrderId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    IsActive = true
                }).ToList();


                await _context.AddRangeAsync(newOrderRows);
                await _context.SaveChangesAsync();


                #region Second approch

                //List<OrderRow> row = new List<OrderRow>();

                //foreach (var item in create.Rows)
                //{
                //    var newOrderRow = new OrderRow
                //    {
                //        OrderItemId = item.OrderItemId,
                //        OrderId = newOrderHead.OrderId,
                //        ProductName = item.ProductName,
                //        Quantity = item.Quantity,
                //        UnitPrice = item.UnitPrice,
                //    };
                //    row.Add(newOrderRow);
                //}

                //await _context.AddRangeAsync(row);
                //await _context.SaveChangesAsync();
                #endregion

                return new MessageHelper
                {
                    message = "success create",
                    statusCode = 200,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<MessageHelper> UpdateOrderWithItem(UpdateOrderDTO update)
        {
            try
            {
                var order = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.OrderId == update.UpdateOrderHead.OrderId);
                

                if (order == null)
                {
                    throw new Exception("Order not found");
                }



                order.CustomerName = update.UpdateOrderHead.CustomerName;
                order.OrderDate = DateTime.Now;
                order.TotalAmount = update.Rows.Sum(x => x.Quantity * x.UnitPrice);
              
                _context.Update(order);
                await _context.SaveChangesAsync();

                List<OrderRow> updateRow = new List<OrderRow>();

                // For Old Item

                var rowData = await _context.OrderRows.Where(x=>x.OrderId==order.OrderId && x.IsActive==true).ToListAsync();

                foreach (var item in rowData)
                {
                    var dt = update.Rows.Where(x => x.OrderItemId == item.OrderItemId).FirstOrDefault();
                    if(dt != null)
                    {
                        item.Quantity=dt.Quantity;
                        item.UnitPrice=dt.UnitPrice;
                    }
                    else
                    {
                        item.IsActive = false;
                    }
                }

                _context.OrderRows.UpdateRange(rowData);
                await _context.SaveChangesAsync();

                // For New Item

                var newRow = update.Rows.Where(x => x.OrderItemId == 0).Select(x => new OrderRow
                {
                    OrderItemId = x.OrderItemId,
                    OrderId = order.OrderId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    IsActive = true
                }).ToList();

                await _context.AddRangeAsync(newRow);
                await _context.SaveChangesAsync();


                return new MessageHelper
                { 
                    message = "success Update", 
                    statusCode = 200,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetOrderDetailsDTO> GetOrderDetails(int Id)
        {

            try
            {
                var orderheader = await (from oh in _context.OrderHeaders
                                         where oh.IsActive == true && oh.OrderId == Id
                                         select new GetOrderDetailsHeaderDTO
                                         {
                                             OrderId = oh.OrderId,
                                             CustomerName = oh.CustomerName,
                                             OrderDate = oh.OrderDate,
                                         }).FirstOrDefaultAsync();
                var orderRow = await (from or in _context.OrderRows
                                      where or.OrderId == Id && or.IsActive == true
                                      select new GetOrderDetailsRowDTO
                                      {
                                          OrderItemId = or.OrderItemId,
                                          ProductName = or.ProductName,
                                          Quantity = or.Quantity,
                                          UnitPrice = or.UnitPrice,
                                      }).ToListAsync();


                var OrderDetails = new GetOrderDetailsDTO
                {
                    getOrderDetailsHeader = orderheader,
                    Rows = orderRow,

                };
                return OrderDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        public async Task<MessageHelper> DeleteOrder(int Id)
        {
            try
            {
                var order = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.OrderId == Id);


                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                order.IsActive = false;

                _context.Update(order);
                await _context.SaveChangesAsync();
                return new MessageHelper
                {
                    message = "Delete success",
                    statusCode = 200,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MaxAndMinDTO> MinimumAndMaximum()
        {
            decimal min = _context.OrderHeaders.Where(row=>row.IsActive==true).Min(row => row.TotalAmount);
            decimal max = _context.OrderHeaders.Where(row => row.IsActive == true).Max(row => row.TotalAmount);

            var MaxMin = new MaxAndMinDTO
            {
                Max = max,
                Min = min,
            };
            return MaxMin;
        }

    }
}
