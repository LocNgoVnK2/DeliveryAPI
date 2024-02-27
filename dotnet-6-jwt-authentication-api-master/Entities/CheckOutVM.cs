namespace WebApi.Entities
{
    public class CheckOutVM
    {
        public int? IdOrder { get; set; }
        public int? CustomerId { get; set; }
        public string? Note { get; set; }
        public bool? IsReceived { get; set; }
    }
}
