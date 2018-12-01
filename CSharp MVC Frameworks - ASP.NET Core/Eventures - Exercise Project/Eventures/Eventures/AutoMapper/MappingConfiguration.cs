using AutoMapper;
using Eventures.Models;
using Eventures.ViewModels.Orders;
using Eventures.ViewModels.Events;
using Eventures.Models.AccountViewModels;
using Eventures.ViewModels.Admin;

namespace Eventures.AutoMapper
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            this.CreateMap<CreateOrderViewModel, Order>();
            this.CreateMap<Order, BaseOrderViewModel>()
               .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.UserName))
               .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name));

            this.CreateMap<CreateEventViewModel, Event>();
            this.CreateMap<Event, BaseEventViewModel>();
            this.CreateMap<Event, MyEventViewModel>();

            this.CreateMap<RegisterViewModel, ApplicationUser>();
            this.CreateMap<ApplicationUser, BaseUserViewModel>();
        }
    }
}
