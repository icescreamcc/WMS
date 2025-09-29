using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.Log;
using Logic.LogicBase;
using Microsoft.Extensions.Configuration;
using Models.Model;
using Models.Model.Baseinfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Logic.BaseInfo
{
    public class FactoryDataSyncMgr
    {
        private readonly string _host;

        private const string _areaDataApi = "/api/area/list";

        private readonly string _areaDataApiToken;

        private const string _lineDataApi = "/api/line/list";

        private readonly string _lineDataApiToken;

        private const string _stationDataApi = "/api/station/list";

        private readonly string _stationDataApiToken;

        private readonly HttpHelperAsync _httpHelperAsync;

        private readonly LogHelper _logHelper;

        private readonly DbContext _dbContext;
        public FactoryDataSyncMgr(HttpHelperAsync httpHelperAsync, DbContext dbContext, IConfiguration configuration, LogHelper logHelper)
        {
            _httpHelperAsync = httpHelperAsync;
            _dbContext = dbContext;
            _logHelper = logHelper;
            _host = configuration.GetSection("DataCenter:Host").Value;
            _areaDataApiToken = configuration.GetSection("DataCenter:AreaDataToken").Value;
            _lineDataApiToken = configuration.GetSection("DataCenter:LineDataToken").Value;
            _stationDataApiToken = configuration.GetSection("DataCenter:StationDataToken").Value;
        }

        public void DataSync()
        {
            try
            {
                var plant = _dbContext.GetDb().Queryable<ProdPlant>().Single(s => s.PlantNo == BusinessConst.PlantNo);
                _logHelper.LogInfo("FactoryDataSyncMgr.DataSync", "开始同步区域、产线、工位数据", plant?.PlantName);
                if (plant != null)
                {
                    var db = _dbContext.GetDb();
                    string areaErr, lineErr, stationErr;
                    var areaData = _getAreaData(plant, out areaErr);
                    var lineData = _getLineData(plant, out lineErr);
                    var stationData = _getStationData(plant, out stationErr);
                    if (areaData != null)
                    {
                        db.Deleteable<ProdArea>().AddQueue();
                        db.Insertable(areaData).AddQueue();
                    }
                    else
                    {
                        _logHelper.LogInfo("FactoryDataSyncMgr.DataSync", $"同步区域数据失败,{areaErr}", null);
                    }
                    if (lineData != null)
                    {
                        db.Deleteable<ProdLine>().AddQueue();
                        db.Insertable(lineData).AddQueue();
                    }
                    else
                    {
                        _logHelper.LogInfo("FactoryDataSyncMgr.DataSync", $"同步产线数据失败,{lineErr}", null);
                    }
                    if (stationData != null)
                    {
                        db.Deleteable<ProdStation>().AddQueue();
                        db.Insertable(stationData).AddQueue();
                    }
                    else
                    {
                        _logHelper.LogInfo("FactoryDataSyncMgr.DataSync", $"同步工位数据失败,{stationErr}", null);
                    }
                    db.SaveQueues();
                    _logHelper.LogInfo("FactoryDataSyncMgr.DataSync", "同步区域、产线、工位数据完成", $"区域数量:{areaData?.Count},产线数量:{lineData?.Count},工位数量:{stationData?.Count}");
                }
            }
            catch(Exception ex)
            {
                _logHelper.LogError("DataSync", ex.Message, "", ex, ex.StackTrace);
            }
        }

        private List<ProdArea> _getAreaData(ProdPlant plant, out string err)
        {
            string url = _host + _areaDataApi;
            err = "";
            var headerDic = new Dictionary<string, string>
            {
                { "authorization", _areaDataApiToken }
            };
            var res = _httpHelperAsync.RequestGet<ResponseDataCenter<List<DataCenterResponse_Area>>>(url, headerDic);
            if (res!=null)
            {
                if (res.data != null && res.data.Length > 0 && res.data[0].result != null && res.data[0].result.Count > 0)
                {
                    return res.data[0].result.Select(s => new ProdArea
                    {
                        PlantNo = plant.PlantNo,
                        AreaNo = s.area_name,
                        AreaName = s.area_name
                    }).ToList();
                }
                else
                {
                    err = res.message;
                    return null;
                }
            }
            return null;
        }

        private List<ProdLine> _getLineData(ProdPlant plant, out string err)
        {
            string url = _host + _lineDataApi;
            err = "";
            var headerDic = new Dictionary<string, string>
            {
                { "authorization", _lineDataApiToken }
            };
            var res = _httpHelperAsync.RequestGet<ResponseDataCenter<List<DataCenterResponse_Line>>>(url, headerDic);
            if (res != null)
            {
                if (res.data != null && res.data.Length > 0 && res.data[0].result != null && res.data[0].result.Count > 0)
                {
                    return res.data[0].result.Select(s => new ProdLine
                    {
                        PlantNo = plant.PlantNo,
                        AreaNo = s.area,
                        LineNo = s.line,
                        LineName = s.line
                    }).ToList();
                }
                else
                {
                    err = res.message;
                    return null;
                }
            }
            return null;
        }

        private List<ProdStation> _getStationData(ProdPlant plant,out string err)
        {
            string url = _host + _stationDataApi;
            err = "";
            var headerDic = new Dictionary<string, string>
            {
                { "authorization", _stationDataApiToken }
            };
            var res = _httpHelperAsync.RequestGet<ResponseDataCenter<List<DataCenterResponse_Station>>>(url, headerDic);
            if (res != null)
            {
                if (res.data != null && res.data.Length > 0 && res.data[0].result != null && res.data[0].result.Count > 0)
                {
                    return res.data[0].result.Select(s => new ProdStation
                    {
                        PlantNo = plant.PlantNo,
                        AreaNo = s.area,
                        LineNo = s.line,
                        StationNo = s.station,
                        StationName = s.station_perform
                    }).ToList();
                }
                else
                {
                    err = res.message;
                    return null;
                }
            }
            return null;
        }


    }
}
