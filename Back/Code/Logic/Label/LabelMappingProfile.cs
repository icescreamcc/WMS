using AutoMapper;
using DbRepository.Repository.DbModels; 
using Models.Model.Label;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Logic.Label
{
    public class LabelMappingProfile: Profile
    {
        public LabelMappingProfile()
        { 
            CreateMap<LabelDesignDto, LabelDesign>().ReverseMap();
            CreateMap<LabelDesignDetailsDto, LabelDesignDetails>().ReverseMap();
            CreateMap<LabelPrintRecordDto, LabelPrintRecord>().ReverseMap();
        }
    }
}
