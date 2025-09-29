using AutoMapper;
using DbRepository.Repository.DbModels; 
using Models.Model.Prod;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ProductOffLine
{
    public class ProdMappingProfile: Profile
    {
        public ProdMappingProfile()
        {
            CreateMap<ProdOrdersDto, ProdOrders > ().ReverseMap();
            CreateMap<ProdOrderDetailsDto, ProdOrderDetails>().ReverseMap();
            CreateMap<Message, SysMessage>().ReverseMap();
            CreateMap<MatchingCarDto, ProdMatchingCar>().ReverseMap();
        }
    }
}
