namespace WebApi.Models;

using WebApi.Entities;

public class AuthenticateResponse
{
    public int AccountID { get; set; }
    //public string FirstName { get; set; }
    //public string LastName { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }

    public int IdStore { get; set; }
    public AuthenticateResponse(UserVM user, string token)
    {
        AccountID = user.AccountID;
        //FirstName = user.FirstName;
        //LastName = user.LastName;
        Username = user.Username;
        Role = user.Role;
        Token = token;
        IdStore = (int)user.IdStore;
    }
}