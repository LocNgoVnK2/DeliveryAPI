using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccountsService _accountsService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly ICheckOutService _checkOutService;
        private readonly IDeliveryDetailService _detailService;


        public OrderController(IMapper mapper,
                                    IAccountsService accountsService,
                                    ICheckOutService checkOutService,
                                    IOrderService orderService,
                                    ICustomerService customerService,
                                    IDeliveryDetailService detailService
                                    )
        {
            _mapper = mapper;
            _accountsService = accountsService;
            _orderService = orderService;
            _customerService = customerService;
            _checkOutService = checkOutService;
            _detailService = detailService;
        }
        [HttpGet("GetOrderNeedToShip")]
        public async Task<IActionResult> GetOrderNeedToShip(int IdStoreofShipper) // load ở view chop nhan vien hoan thanh dơn hang

        {

            List<CheckOutBillViewModel> checkOutViewModels = new List<CheckOutBillViewModel>();

            var orders = await _orderService.GetOrders();
            orders = orders.Where(e => e.IsDone == true && e.IdStore == IdStoreofShipper && e.DeliveryId == null).Select(e => e);

            foreach (Order order in orders)
            {
                CheckOut checkOut = await _checkOutService.GetCheckOut((int)order.OrderID);

                if (checkOut != null && checkOut.IsReceived == false)
                {
                    Customer customer = await _customerService.GetCustomer((int)checkOut.CustomerId);
                    CheckOutBillViewModel checkOutView = new CheckOutBillViewModel()
                    {

                        IdOrder = checkOut.IdOrder,
                        IsReceived = checkOut.IsReceived,
                        TotalPrice = order.TotalAmount + order.ShippingFee,
                        OrderDate = order.OrderDate,
                        PhoneNumber = customer.PhoneNumber,
                        Address = customer.Address,
                        FirstName = customer.FirstName + " " + customer.LastName,
                        IdStore = order.IdStore,
                        Note = checkOut.Note,
                        PaymentStatus = (bool)order.PaidStatus,
                        DeliveryId = order.DeliveryId,
                        ShippingFee = order.ShippingFee,

                    };
                    checkOutViewModels.Add(checkOutView);

                }

            }

            return Ok(checkOutViewModels);

        }
        // delicery id == null có nghĩa là đơn này chưa dc pick bởi shipper nào 
        // nếu đã dc pick tạo cột bên delivery detail và set lại và set thêm id vào ordeer và set lại time received, khi complete thì them hinh va time complte va set status =1 
        [HttpPost("PickupOrder")]
        public async Task<IActionResult> PickupOrder(int AccountId, int orderId) // load ở view chop nhan vien hoan thanh dơn hang
        {

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
               
                    DeliveryDetail deliveryDetail = new DeliveryDetail()
                    {
                        AccountID = AccountId,
                        DeliveryStatus = false,
                        TimeReceived = DateTime.UtcNow,
                    };

                    await _detailService.InsertDeliveryDetail(deliveryDetail);
                    Order order = await _orderService.GetOrder(orderId);
                    order.DeliveryId = deliveryDetail.DeliveryId;
                    await _orderService.UpdateOrder(order);
                    transactionScope.Complete();

                    return Ok("Success");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while processing the request.");
                }
            }


        }
        // nếu đã dc pick tạo cột bên delivery detail và set lại và set thêm id vào ordeer và set lại time received,
        // khi complete thì them hinh va time complte va set status =1
        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(DeliveryDetailVM deliveryDetailVM) // load ở view chop nhan vien hoan thanh dơn hang
        {
            // Bug mất dât cũ khi update
            try
            {
                DeliveryDetail deliveryDetail = _mapper.Map<DeliveryDetail>(deliveryDetailVM);
                deliveryDetail.DeliveryStatus = true;
                deliveryDetail.TimeComplete = DateTime.Now;
                await _detailService.UpdateDeliveryDetail(deliveryDetail);
                return Ok();
            }catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request. :"+ ex);
            }

        }
    }
}
