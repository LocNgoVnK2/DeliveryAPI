namespace WebApi.Services;

using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models;

public interface IUserService
{
    Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(int id);
}

public class UserService : IUserService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    /*
    private List<User> _users = new List<User>
    {
        new User { Id = 1, Username = "test", Password = "test" }
    };
    */
    

    private readonly AppSettings _appSettings;
    private readonly IAccountsService _accountService;
    private readonly IMapper _mapper;
    public UserService(IOptions<AppSettings> appSettings, IAccountsService accountsService,IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _accountService = accountsService;
        _mapper = mapper;
    }

    public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
    {
        var _users = await _accountService.GetAccounts();
        User userLogin = new User();
        /*
        var userExist = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
        */
        Accounts user = _users.Where(e => e.Username == model.Username).FirstOrDefault();
        if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password) && user.IsActivate == true)
        {
            userLogin.Username = user.Username;
            userLogin.AccountID = (int)user.AccountID;
            userLogin.Password = user.Password;
            userLogin.Role = user.Role;
        }
        else
        {
            return null;
        }
        // authentication successful so generate jwt token
        var token = generateJwtToken(userLogin);

        return new AuthenticateResponse(userLogin, token);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var _listusers = await _accountService.GetAccounts();
        List<User> _users = new List<User>();
        _users = _mapper.Map<List<User>>(_listusers);
        return _users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var _userRes = await _accountService.GetAccount(id);
        User user = new User() { 
            AccountID = (int)_userRes.AccountID,
            Username = _userRes.Username,
            Password = _userRes.Password,
            Role = _userRes.Role,
        };
        return user;
    }

    // helper methods

    private string generateJwtToken(User user)
    {
     
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.AccountID.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}