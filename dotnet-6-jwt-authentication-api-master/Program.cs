using Infrastructure.EF;
using Infrastructure.Repository;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using WebApi;
using WebApi.Helpers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    services.AddCors();
    services.AddControllers();
    //account
    services.AddScoped<IAccountsRepository, AccountsRepository>();

    services.AddScoped<IAccountsService, AccountsService>();
    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IUserService, UserApplicationService>();

    //Order
    services.AddScoped<IOrderRepository, OrderRepository>();
    services.AddScoped<IOrderService, OrderService>();

    // customer
    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<ICustomerService, CustomerService>();

    //checkout 
    services.AddScoped<ICheckOutRepository, CheckOutRepository>();
    services.AddScoped<ICheckOutService, CheckOutService>();
    //DeliveryDetail
    services.AddScoped<IDeliveryDetailService, DeliveryDetailService>();
    services.AddScoped<IDeliveryDetailRepository, DeliveryDetailRepository>();



    services.AddDbContext<EXDbContext>(options =>
    {

        options.UseSqlServer(builder.Configuration.GetConnectionString("EXDbContextConnection"));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
    services.AddAutoMapper(typeof(Mapping));
}


var app = builder.Build();

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");