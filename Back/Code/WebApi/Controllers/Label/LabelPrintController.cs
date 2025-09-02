using Logic.Label;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model.Label;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filter;

namespace WebApi.Controllers.Label
{ 
    public class LabelPrintController : AuthTokenController
    {
        private LabelPrintMgr _labelPrintMgr;

        public LabelPrintController(LabelPrintMgr labelPrintMgr)
        {
            _labelPrintMgr = labelPrintMgr;
        }

        [HttpGet]
        [Skip]
        public async Task<List<LabelDesignDto>> GetLabels(string goodsClassifyGroup)
        {
            return await _labelPrintMgr.GetLabels(goodsClassifyGroup);
        }

        [HttpGet]
        [Skip]
        public async Task<List<LabelDesignDetailsDto>> GetLableDesignDetails(string labelId)
        {
            return await _labelPrintMgr.GetLableDesignDetails(labelId);
        }

        [HttpPost]
        public async Task AddPrintRecord(LabelPrintRecordDto data)
        {
            await _labelPrintMgr.AddPrintRecord(data);
        }

        [HttpGet]
        [Skip]
        public  List<LabelPrintRecordDto> GetLabelRecord(int pgSize, int pgIndex, string orderFiled, string orderType, string goodsClassifyGroup, string searchKey, string dateStart, string dateEnd)
        {
            return  _labelPrintMgr.GetLabelRecord(pgSize, pgIndex, orderFiled, ConvertOrderType(orderType, false), goodsClassifyGroup, searchKey, dateStart, dateEnd);
        }
    }
}
