namespace WebApi.Entities
{
    public class CustomerVM
    {
        public int? CustomerId { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ValidationCode { get; set; }
    }
}
