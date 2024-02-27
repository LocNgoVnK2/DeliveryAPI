namespace WebApi.Entities
{
    public class OrderVM
    {
        public int? OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public Double? TotalAmount { get; set; }
        public Double? ShippingFee { get; set; }
        public int? AccountId { get; set; } // khi account này khác null có nghĩa là đơn hàng đã dc xác nhận 
        public bool? IsDone { get; set; }
        public bool? PaidStatus { get; set; }
        public int? IdStore { get; set; }
        //DeliveryId
        public Double? DiscountMoney { get; set; }
        public int? DeliveryId { get; set; }
    }

}
