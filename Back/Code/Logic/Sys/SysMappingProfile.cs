using AutoMapper;
using DbRepository.Repository.DbModels;
using Models.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Sys
{
   public class SysMappingProfile: Profile
    {
        public SysMappingProfile()
        {
            CreateMap<ExternalProviderDto, SysExternalProvider>().ReverseMap();
            CreateMap<UserDetail, SysUser>().ReverseMap();
            CreateMap<Company, SysCompany>().ReverseMap();
            CreateMap<Department, SysDepartments>().ReverseMap();
            CreateMap<ApprovalHisModel, ApprovalHis>().ReverseMap();
            CreateMap<MailDto, SysMail>().ReverseMap();
        }
    }
}
