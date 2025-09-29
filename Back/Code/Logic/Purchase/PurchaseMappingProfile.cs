using AutoMapper;
using DbRepository.Repository.DbModels;
using Models.Model.Purchase;

namespace Logic.Purchase
{
    public class PurchaseMappingProfile: Profile
    {
        public PurchaseMappingProfile()
        {
            CreateMap<ReceivingOrderDto, ReceivingOrder>().ReverseMap(); 
            CreateMap<ReceivingOrderDetailDto, ReceivingOrderDetail>().ReverseMap();
            CreateMap<ReceivingOrderDetailDto, ReceivingOrderExpandDto>().ReverseMap();
            CreateMap<SendingOrderDto, SendingOrder>().ReverseMap();
            CreateMap<SendingOrderDetailDto, SendingOrderDetail>().ReverseMap();
            CreateMap<SendingOrderDetailDto, SendingOrderExpandDto>().ReverseMap();
            CreateMap<PurchaseOrderDto, PurchaseOrder>().ReverseMap();
        }
    }
}
