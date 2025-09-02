using AutoMapper;
using DbRepository.Repository.DbModels;
using Models.Model.Plan; 

namespace Logic.PlanMaterial
{
    public class PlanMappingProfile : Profile
    {
        public PlanMappingProfile()
        {
            CreateMap<PlanFinishedProductOrderDto, PlanFinishedProductOrder>().ReverseMap();
            CreateMap<PlanMaterialRequirementOrderDto, PlanMaterialRequirementOrder>().ReverseMap();
            CreateMap<PlanMaterialRequirementDetailDto, PlanMaterialRequirementDetail>().ReverseMap();
        }

    }
}
