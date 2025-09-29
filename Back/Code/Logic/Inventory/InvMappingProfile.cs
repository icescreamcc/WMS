using AutoMapper;
using DbRepository.Repository.DbModels;
using Models.Model.Inv;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Inventory
{
    public class InvMappingProfile: Profile
    {
        public InvMappingProfile()
        {
            CreateMap<WarehouseDetail, InvWarehouse>().ReverseMap();
            CreateMap<Shelf, InvShelf>().ReverseMap();
            CreateMap<Bin, InvBin>().ReverseMap();
            CreateMap<WorkbinDto,InvWorkbin>().ReverseMap();
            CreateMap<WorkbinSpecificationDto,InvWorkbinSpecification>().ReverseMap();
            CreateMap<RequisitionOrderDto, InvRequisitionOrder>().ReverseMap();
            CreateMap<RequisitionOrderDetailDto, InvRequisitionOrderDetail>().ReverseMap(); 
            CreateMap<InStorageLabelsDto, InvInStorageLabels>().ReverseMap();
            CreateMap<InvSafetyWarningRecordDto, InvSafetyWarningRecord>().ReverseMap();
        }
    }
}
