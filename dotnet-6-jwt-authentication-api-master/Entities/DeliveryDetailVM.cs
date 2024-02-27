namespace WebApi.Entities
{
    public class DeliveryDetailVM
    {
        public int DeliveryId { get; set; }
        public int? AccountID { get; set; }
        public bool? DeliveryStatus { get; set; }
        public DateTime? TimeReceived { get; set; }
        public byte[]? PickUpPhoto { get; set; }
        public DateTime? TimeComplete { get; set; }
    }
}
