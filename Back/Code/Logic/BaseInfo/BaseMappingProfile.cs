using AutoMapper;
using DbRepository.Repository.DbModels;
using Models.Model.Baseinfo;  

namespace Logic.BaseInfo
{
    public class BaseMappingProfile : Profile
    {
        public BaseMappingProfile()
        {
            CreateMap<SparePartDto, BaseGoods>().ReverseMap();
            CreateMap<SamplePieceDto, BaseGoods>().ReverseMap(); 
            CreateMap<SeparatorDto, BaseGoods>().ReverseMap(); 
            CreateMap<PackingMaterialDto, BaseGoods>().ReverseMap();
            CreateMap<RawMaterialDto, BaseGoods>().ReverseMap();
            CreateMap<FinishedProductDto, BaseGoods>().ReverseMap();
            CreateMap<ConsumablesDto, BaseGoods>().ReverseMap();
            CreateMap<BOMDto, BaseBOM>().ReverseMap();
            CreateMap<SupplierDto, BaseSuppliers>().ReverseMap(); 
            CreateMap<SupplierContactDto, BaseSupplierContact>().ReverseMap();
            CreateMap<SupplierAccountCreditedDto, BaseSupplierAccountCredited>().ReverseMap();
            CreateMap<FileInfoDto, BaseFiles>().ReverseMap();
        }
    }
}
