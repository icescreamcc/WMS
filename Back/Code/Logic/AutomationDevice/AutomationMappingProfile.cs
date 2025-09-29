using AutoMapper;
using DbRepository.Repository.DbModels;
using Models.Model.AutomationDevice; 

namespace Logic.AutomationDevice
{
    public class AutomationMappingProfile: Profile
    {
        public AutomationMappingProfile() 
        { 
            CreateMap<AutoProdDeviceDto, AutoProdDevice>().ReverseMap();
            CreateMap<AutoProdDeviceWarehouseDto, AutoProdDeviceWarehouse>().ReverseMap();
            CreateMap<AutoProdDeviceAGVDto, AutoProdDeviceAGV>().ReverseMap();
            CreateMap<AutoProdTaskTrackingDto, AutoProdTaskTracking>().ReverseMap();
        }
    }
}
