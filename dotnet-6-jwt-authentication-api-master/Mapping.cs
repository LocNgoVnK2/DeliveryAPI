using AutoMapper;
using Infrastructure.Entities;
using WebApi.Entities;

namespace WebApi
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            // account
            CreateMap<Accounts, UserVM>().ReverseMap();
            CreateMap<CheckOutVM,CheckOut>().ReverseMap();
            CreateMap<DeliveryDetailVM,DeliveryDetail>().ReverseMap();
        }
    }
}
