namespace WebApi.Entities;

using System.Text.Json.Serialization;

public class UserVM
{
    /*
        public int? AccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? ValidationCode { get; set; }
        public string Role { get; set; }
        public bool? IsActivate { get; set; }
        public int? EmployeeID { get; set; }
        public int? IdStore { get; set; }
     */
    public int AccountID { get; set; }

    public string Username { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
    public string Role { get; set; }
    public int? IdStore { get; set; }
}