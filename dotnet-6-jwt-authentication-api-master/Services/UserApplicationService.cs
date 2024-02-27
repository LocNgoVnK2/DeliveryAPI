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
    Task<IEnumerable<UserVM>> GetAllAsync();
    Task<UserVM> GetByIdAsync(int id);
}

public class UserApplicationService : IUserService
{
    private readonly AppSettings _appSettings;
    private readonly IAccountsService _accountService;
    private readonly IMapper _mapper;
    public UserApplicationService(IOptions<AppSettings> appSettings, IAccountsService accountsService,IMapper mapper)
    {
        _appSettings = appSettings.Value;
        _accountService = accountsService;
        _mapper = mapper;
    }

    public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
    {
        var _users = await _accountService.GetAccounts();
        UserVM userLogin = new UserVM();
    
        Accounts user = _users.Where(e => e.Username == model.Username).FirstOrDefault();
        if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password) && user.IsActivate == true)
        {
            userLogin.Username = user.Username;
            userLogin.AccountID = (int)user.AccountID;
            userLogin.Password = user.Password;
            userLogin.Role = user.Role;
            userLogin.IdStore = user.IdStore;
        }
        else
        {
            return null;
        }
        // authentication successful so generate jwt token
        var token = generateJwtToken(userLogin);

        return new AuthenticateResponse(userLogin, token);
    }

    public async Task<IEnumerable<UserVM>> GetAllAsync()
    {
        var _listusers = await _accountService.GetAccounts();
        List<UserVM> _users = new List<UserVM>();
        _users = _mapper.Map<List<UserVM>>(_listusers);
        return _users;
    }

    public async Task<UserVM> GetByIdAsync(int id)
    {
        var _userRes = await _accountService.GetAccount(id);
        UserVM user = new UserVM() { 
            AccountID = (int)_userRes.AccountID,
            Username = _userRes.Username,
            Password = _userRes.Password,
            Role = _userRes.Role,
        };
        return user;
    }

    // helper methods

    private string generateJwtToken(UserVM user)
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