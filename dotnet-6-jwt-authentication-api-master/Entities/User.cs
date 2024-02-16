namespace WebApi.Entities;

using System.Text.Json.Serialization;

public class User
{
    /*
             [Key]
        [Column("AccountID")]
        public int? AccountID { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("Password")]
        public string Password { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("ValidationCode")]
        public string? ValidationCode { get; set; }

        [Column("Role")]
        public string Role { get; set; }

        [Column("IsActivate")]
        public bool? IsActivate { get; set; }
        [Column("EmployeeID")]
        public int? EmployeeID { get; set; }

        public int? IdStore { get; set; }

     */
    public int AccountID { get; set; }

    public string Username { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
    public string Role { get; set; }
}