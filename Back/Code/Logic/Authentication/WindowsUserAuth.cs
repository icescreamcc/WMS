 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DbRepository.Repository.DbModels;
using DbRepository.Repository;
using Models.Model.Sys;
using Logic.LogicBase;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices.JavaScript;

namespace Logic.Authentication
{
    public class WindowsUserAuth: DbOperationHandler
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;

        public WindowsUserAuth(HttpClient httpClient, Repository repository, IConfiguration configuration) :base(repository)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public string GetTicketUrl()
        {
            var Enable = "";
            var getTicketUrl = "";
            var sign = _configuration.GetSection("Factory:sign").Value;
            if (sign == "dl")
            {
                Enable = _configuration.GetSection("CasServer:Enable").Value;
                getTicketUrl = _configuration.GetSection("CasServer:GetTicketUrl").Value;
            }
            else
            if (sign == "w3")
            {
                Enable = _configuration.GetSection("CasServerW3:Enable").Value;
                getTicketUrl = _configuration.GetSection("CasServerW3:GetTicketUrl").Value;
            }
            var isEnable = bool.Parse(Enable);
            if (isEnable)
            {
                return getTicketUrl;
            }
                
            else return null;
        }

        public async Task<UserAuthorizationDto> GetUserAccount(string ticket)
        {
            var getAccountUrl = "";
            var sign = _configuration.GetSection("Factory:sign").Value;
            if (sign == "dl")
            {
                getAccountUrl = _configuration.GetSection("CasServer:GetAccountUrl").Value;

            }
            else
            if (sign == "w3")
            {
                getAccountUrl = _configuration.GetSection("CasServerW3:GetAccountUrl").Value;
            }
            //var getAccountUrl = _configuration.GetSection("CasServer:GetAccountUrl").Value;
            var response = await _httpClient.GetAsync(getAccountUrl + ticket);
            var userAccount = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(userAccount))
            {
                return await Repository.ClientDb.Queryable<SysUser>()
                 .LeftJoin<SysDepartments>((u, d) => u.DeptId == d.DeptId)
                 .Where((u) => u.UserId == userAccount ||u.AuthAccount== userAccount)
                 .Select((u, d) => new UserAuthorizationDto { UserId = u.UserId, UserName = u.UserName, NickName = u.NickName, DeptId = d.DeptId, DeptName = d.DeptName, IsVaild = u.IsVaild }).SingleAsync();
            }
            return null;
        }
    }
}
