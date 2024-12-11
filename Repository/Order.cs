using CRUD_Task_03.DBContext;
using CRUD_Task_03.DTO;
using CRUD_Task_03.Helper;
using CRUD_Task_03.IRepository;
using CRUD_Task_03.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using static CRUD_Task_03.DTO.MinMaxDTO;
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
                    OrderId = newOrderHead.OrderId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    IsActive = true
                }).ToList();


                await _context.AddRangeAsync(newOrderRows);
                await _context.SaveChangesAsync();
              

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

                rowData = rowData.Select(item =>
                {
                    var dt = update.Rows.Where(x => x.OrderItemId == item.OrderItemId).FirstOrDefault();
                    if (dt != null)
                    {
                        item.Quantity = dt.Quantity;
                        item.UnitPrice = dt.UnitPrice;
                    }
                    else
                    {
                        item.IsActive = false;
                    }
                    return item;
                }).ToList();


                //foreach (var item in rowData)
                //{
                //    var dt = update.Rows.Where(x => x.OrderItemId == item.OrderItemId).FirstOrDefault();
                //    if (dt != null)
                //    {
                //        item.Quantity = dt.Quantity;
                //        item.UnitPrice = dt.UnitPrice;
                //    }
                //    else
                //    {
                //        item.IsActive = false;
                //    }
                //}

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
                                         where oh.OrderId == Id && oh.IsActive == true
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
            try
            {
                decimal min = _context.OrderHeaders.Where(row => row.IsActive == true).Min(row => row.TotalAmount);
                decimal max = _context.OrderHeaders.Where(row => row.IsActive == true).Max(row => row.TotalAmount);

                var MaxMin = new MaxAndMinDTO
                {
                    Max = max,
                    Min = min,
                };
                return MaxMin;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<MinMaxDTO> MaxAndMin()
        {
            try
            {
                var minOrder = await _context.OrderHeaders.Where(h => h.IsActive == true)
                                                          .OrderBy(h => h.TotalAmount)
                                                          .FirstOrDefaultAsync();

                var minOrderHeader = new GetOrderDetailsHeaderDTO
                {
                    OrderId = minOrder.OrderId,
                    CustomerName = minOrder.CustomerName,
                    OrderDate = minOrder.OrderDate,
                };

                var minOrderRows = await _context.OrderRows.Where(r => r.IsActive == true && r.OrderId == minOrder.OrderId)
                                                     .Select(row => new GetOrderDetailsRowDTO
                                                     {
                                                         OrderItemId = row.OrderItemId,
                                                         ProductName = row.ProductName,
                                                         Quantity = row.Quantity,
                                                         UnitPrice = row.UnitPrice,
                                                     }).ToListAsync();

                var maxOrder = await _context.OrderHeaders.Where(h => h.IsActive == true)
                                                    .OrderByDescending(h => h.TotalAmount)
                                                    .FirstOrDefaultAsync();

                var maxOrderHeader = new GetOrderDetailsHeaderDTO
                {
                    OrderId = maxOrder.OrderId,
                    CustomerName = maxOrder.CustomerName,
                    OrderDate = maxOrder.OrderDate,
                };

                var maxOrderRows = await _context.OrderRows.Where(r => r.IsActive == true && r.OrderId == maxOrder.OrderId)
                                                     .Select(row => new GetOrderDetailsRowDTO
                                                     {
                                                         OrderItemId = row.OrderItemId,
                                                         ProductName = row.ProductName,
                                                         Quantity = row.Quantity,
                                                         UnitPrice = row.UnitPrice,
                                                     }).ToListAsync();
                return new MinMaxDTO
                {
                    MinOrderDetails = new GetOrderDetailsHeaderDTO
                    {
                        OrderId = minOrder.OrderId,
                        CustomerName = minOrder.CustomerName,
                        OrderDate = minOrder.OrderDate,
                        Rows = minOrderRows,
                    },

                    MaxOrderDetails = new GetOrderDetailsHeaderDTO
                    {
                        OrderId= maxOrder.OrderId,
                        CustomerName = maxOrder.CustomerName,
                        OrderDate = maxOrder.OrderDate,
                        Rows = maxOrderRows,
                    }
                };

          


            }
            catch (Exception ex)
            {
                throw;
            }
        }   
        public async Task<List<GetOrderDetailsHeaderDTO>> SearchByCustormerName(string name)
        {
            try
            {
                var customer = await _context.OrderHeaders.Where(x => x.CustomerName.Contains(name)
                                                       && x.IsActive == true).Select(o => new GetOrderDetailsHeaderDTO
                                                       {
                                                           CustomerName = o.CustomerName,
                                                           OrderDate = o.OrderDate,
                                                           OrderId = o.OrderId,
                                                           Rows = _context.OrderRows.Where(x=>x.OrderId ==  o.OrderId && x.IsActive == true)
                                                                                    .Select(y => new GetOrderDetailsRowDTO {
                                                                                        OrderItemId = y.OrderItemId,
                                                                                        ProductName = y.ProductName,
                                                                                        Quantity = y.Quantity,
                                                                                        UnitPrice = y.UnitPrice,
                                                                                    }).ToList()
                                                                                                                           
                                                       }).ToListAsync();
                
                //foreach (var item in customer)
                //{
                //    item.Rows = await _context.OrderRows.Where(x => x.IsActive == true
                //                                        && x.OrderId == item.OrderId).Select(y => new GetOrderDetailsRowDTO
                //                                        {
                //                                            OrderItemId = y.OrderItemId,
                //                                            ProductName = y.ProductName,
                //                                            Quantity = y.Quantity,
                //                                            UnitPrice = y.UnitPrice,
                //                                        }).ToListAsync();
                //}

                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public async Task<List<GetOrderDetailsHeaderDTO>> DateRang(DateTime fromDate , DateTime ToDate)
        {

            try
            {
                var Order = await _context.OrderHeaders.Where(x => x.OrderDate.Date >= fromDate.Date && x.OrderDate.Date <= ToDate.Date
                                                   && x.IsActive == true).Select(o => new GetOrderDetailsHeaderDTO
                                                   {
                                                       CustomerName = o.CustomerName,
                                                       OrderDate = o.OrderDate,
                                                       OrderId = o.OrderId,
                                                       Rows = _context.OrderRows.Where(x=>x.OrderId == o.OrderId && x.IsActive == true)
                                                                                .Select(y=>new GetOrderDetailsRowDTO
                                                                                {
                                                                                    OrderItemId = y.OrderItemId,
                                                                                    ProductName = y.ProductName,
                                                                                    Quantity = y.Quantity,
                                                                                    UnitPrice = y.UnitPrice,
                                                                                }).ToList()

                                                   }).ToListAsync();

                //foreach (var item in Order)
                //{
                //    item.Rows = await _context.OrderRows.Where(x => x.OrderId == item.OrderId
                //                                        && x.IsActive == true).Select(y => new GetOrderDetailsRowDTO
                //                                        {
                //                                            OrderItemId = y.OrderItemId,
                //                                            ProductName = y.ProductName,
                //                                            Quantity = y.Quantity,
                //                                            UnitPrice = y.UnitPrice,
                //                                        }).ToListAsync();
                //}

                return Order;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<DateRangTotalAmountDTO> DateRangTotalAmount(DateTime fromDate, DateTime ToDate)
        {

            try
            {
                var TotalSum = await _context.OrderHeaders.Where(x => x.OrderDate >= fromDate.Date && x.OrderDate <= ToDate.Date
                                                   && x.IsActive == true).SumAsync(p => p.TotalAmount);

                var sum = new DateRangTotalAmountDTO
                {
                    TotalSum = TotalSum,
                };
                return sum;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<DailyTotalSalesDTO>> DailyTotalSales ()
        {
            try
            {
               
               var dailySales = await _context.OrderHeaders
                                      .GroupBy(o => o.OrderDate.Date) 
                                      .Select(g => new DailyTotalSalesDTO
                                      {
                                         OrderDate = g.Key,
                                         TotalSales = g.Sum(o => o.TotalAmount)
                                      })
                                      .OrderBy(result => result.OrderDate) 
                                      .ToListAsync();

                return dailySales;

                
             }



            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<MessageHelper> CreateOrdersWithItemBulkInsert(List<CreateOrderDTO> createOrders)
        {


            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var orderHeaders = new List<OrderHeader>();
                var orderRows = new List<OrderRow>();



                var tasks = createOrders.Select(async create =>
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

                    var newOrderRows = create.Rows.Select(x => new OrderRow
                    {
                        OrderId = newOrderHead.OrderId,
                        ProductName = x.ProductName,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        IsActive = true
                    }).ToList();

                    orderRows.AddRange(newOrderRows);
                });
                await Task.WhenAll(tasks);
                await _context.AddRangeAsync(orderRows);
                await _context.SaveChangesAsync();

                


                await transaction.CommitAsync();

                return new MessageHelper
                {
                    message = "Orders created successfully",
                    statusCode = 200,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }

            //using var transaction = await _context.Database.BeginTransactionAsync();
            //try
            //{
            //    var allHeaders = createOrders.Select(create => new OrderHeader
            //    {
            //        CustomerName = create.CreateOrderHead.CustomerName,
            //        OrderDate = DateTime.Now,
            //        TotalAmount = create.Rows.Sum(item => item.Quantity * item.UnitPrice),
            //        IsActive = true,
            //    }).ToList();

            //    await _context.OrderHeaders.AddRangeAsync(allHeaders);
            //    await _context.SaveChangesAsync();

            //    var allRows = createOrders.SelectMany((create, index) => create.Rows.Select(item => new OrderRow
            //    {
            //        OrderId = allHeaders[index].OrderId,
            //        ProductName = item.ProductName,
            //        Quantity = item.Quantity,
            //        UnitPrice = item.UnitPrice,
            //        IsActive = true,
            //    })).ToList();

            //    await _context.OrderRows.AddRangeAsync(allRows);
            //    await _context.SaveChangesAsync();

            //    await transaction.CommitAsync();

            //    return new MessageHelper
            //    {
            //        message = "Successfully Created Order",
            //        statusCode = 200,
            //    };
            //}
            //catch (Exception)
            //{
            //    await transaction.RollbackAsync();
            //    throw;
            //}


        }    
        public async Task<List<OrderListDTO>> OrdersListBypagination(int pageNo, int PageSize)
        {
            try
            {
                var orderList = await _context.OrderHeaders.Where(x=>x.IsActive == true).Skip((pageNo-1)*PageSize).Take(PageSize).ToListAsync();
                                            
                var finalList = new List<OrderListDTO>();

                foreach (var order in orderList)
                {
                    finalList.Add(new OrderListDTO
                    {

                        CustomerName = order.CustomerName,
                        OrderDate = order.OrderDate,
                        TotalAmount = order.TotalAmount

                    });
                }

                return finalList;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public async  Task<List<GetOrderDetailsHeaderrDTO>> FiltersByDateTimeMinMaxCustormerName(OrderFilterDTO filter)
        {
           
            try
            {
                
                var orderHeaders = await _context.OrderHeaders.Where(x => x.IsActive == true
                                             && (filter.StartDate == null || x.OrderDate.Date>=filter.StartDate.Value.Date) 
                                             && (filter.EndDate == null || x.OrderDate <= filter.EndDate.Value.Date)
                                             && (filter.CustomerName == null || x.CustomerName.Contains(filter.CustomerName.Trim()))
                                             && (filter.MinTotalAmount == null || x.TotalAmount >= filter.MinTotalAmount)
                                             && (filter.MaxTotalAmount==null || x.TotalAmount <= filter.MaxTotalAmount))
                                             .Select(o => new GetOrderDetailsHeaderrDTO
                                             {
                                                 CustomerName = o.CustomerName,
                                                 OrderDate = o.OrderDate,
                                                 OrderId = o.OrderId,

                                             }).ToListAsync();

                var orderIds = orderHeaders.Select(o => o.OrderId).ToList();

                var orderRows = await _context.OrderRows
                    .Where(x => orderIds.Contains(x.OrderId) && x.IsActive == true)
                    .Select(y => new GetOrderDetailsRowwDTO
                    {
                        OrderItemId = y.OrderItemId,
                        ProductName = y.ProductName,
                        Quantity = y.Quantity,
                        UnitPrice = y.UnitPrice,
                        OrderId = y.OrderId 
                    })
                    .ToListAsync();

                
                var updatedOrderHeaders = orderHeaders.Select(header =>
                {
                    header.Rows = orderRows.Where(r => r.OrderId == header.OrderId).ToList();
                    return header;
                }).ToList();

                return orderHeaders;
            }
           catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DailyTotalSalesDTO>> GetByDateTimeTotalAmountRang(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var res =await _context.OrderHeaders
                                      .GroupBy(o => o.OrderDate.Date)
                                      .Select(g => new DailyTotalSalesDTO
                                      {
                                          OrderDate = g.Key,
                                          TotalSales = g.Sum(o => o.TotalAmount)
                                      })
                                      .OrderBy(result => result.OrderDate)
                                      .ToListAsync();

                return res;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    
}
