using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common;
using External.HIK_RCS;
using External.HIK_RCS.Dto;
using External.Log;
using External.Socket.PLC;
using External.Socket.Socket;
using Logic.AutomationDevice.PLCConfig;
using Logic.LogicBase;
using Models.Model;
using Models.Model.AutomationDevice;
using Models.Model.Enum;
using Models.Model.Sys;
using Newtonsoft.Json;
using NPOI.SS.Formula.PTG;
using S7.Net;
using SqlSugar;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Logic.AutomationDevice
{
    public class AutoTransportHandler : IDisposable
    {
        private readonly DbContext _dbContext;

        private readonly WebSocketService _webSocketService;

        private readonly RCSService _rcsService;

        private static List<SiemensS7Client>?  _siemensS7Clients;

        private static List<AutoProdDevicePlcConfig>?  _autoProdDevicePlcConfigs;

        private static List<AutoProdDeviceDto>? _plcDeviceInfo;

        private readonly LogHelper _logHelper;

        private readonly IMapper _mapper; 

        private static bool _tasklocker = false;

        private object _locker = new object();

        private bool _disposed = false;

        public AutoTransportHandler(DbContext dbContext,
            WebSocketService webSocketService, 
            RCSService rcsService,   
            LogHelper logHelper,
            IMapper mapper
            )
        {
            _dbContext = dbContext;
            _webSocketService = webSocketService;
            _rcsService = rcsService;  
            _logHelper = logHelper;
            _mapper = mapper;
            _webSocketService.OnDataReceived += _processWebSocketReceive;
            if (!_rcsService.IsAgvCallbackSubscribed)
            {
                _rcsService.OnTaskResult += _onAgvCallback;
                _rcsService.IsAgvCallbackSubscribed = true;
            }
            if (_siemensS7Clients == null)
            {
                lock (_locker)
                    _siemensS7Clients = new List<SiemensS7Client>();
            }
        }

        /// <summary>
        /// 接收前端消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _processWebSocketReceive(object? sender, WebSocketArgs e)
        {
            if (!string.IsNullOrEmpty(e.Message))
            {
                var res = JsonConvert.DeserializeObject<SocketMessageModel<string>>(e.Message);
                if (res != null)
                {
                    //查询并反馈已建立连接的PLC
                    if (res.Type == SocketMessageType.ConnectQuery.ToString())
                    {
                        if (_plcDeviceInfo != null && _siemensS7Clients != null)
                        {
                            foreach (var device in _plcDeviceInfo)
                            {
                                var plc = _siemensS7Clients.SingleOrDefault(s => s.DeviceNo == device.DeviceNo);
                                if (plc != null && plc.IsConnected)
                                {
                                    var msg = new SocketMessageModel<string>
                                    {
                                        Name = device.DeviceNo,
                                        Message = "连接成功!",
                                        Address = device.ConnectAddress,
                                        Status = SocketMessageStatus.Success.ToString(),
                                        Type = SocketMessageType.Connect.ToString(),
                                        Date = DateTime.Now
                                    };
                                    _processWebSocketSend(msg);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 推送消息给前端
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private void _processWebSocketSend<T>(SocketMessageModel<T> data)
        {
            try
            {
                _ = _webSocketService.SendMassage(data);
            }
            catch (Exception ex)
            { 
                throw new BusinessException("与客户端通讯失败，错误码[WebSocket-SendFailed]，详细请查看系统日志");
            }
        }
      
        /// <summary>
        /// 连接所有设备PLC
        /// </summary>
        /// <param name="plcDeviceArr"></param>
        /// <exception cref="BusinessException"></exception>
        public void ConnectAllPLC(List<AutoProdDeviceDto> plcDeviceArr)
        {
            _plcDeviceInfo = plcDeviceArr;
            if (_siemensS7Clients == null)
            {
                _siemensS7Clients = new List<SiemensS7Client>();
            }
            if (_autoProdDevicePlcConfigs == null)
            {
                _autoProdDevicePlcConfigs = _dbContext.GetDb().Queryable<AutoProdDevicePlcConfig>().Where(s=>s.DeviceType == AutoProdDeviceType.Conveyor.ToString()).ToList();
            }
            foreach (var device in plcDeviceArr)
            {
                ConnectSinglePLC(device);
            }
        }

        /// <summary>
        /// 连接指定设备PLC
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="BusinessException"></exception>
        public void ConnectSinglePLC(AutoProdDeviceDto device)
        {
            if (_siemensS7Clients == null)
            {
                _siemensS7Clients = new List<SiemensS7Client>();
            }
            if (_autoProdDevicePlcConfigs == null)
            {
                _autoProdDevicePlcConfigs = _dbContext.GetDb().Queryable<AutoProdDevicePlcConfig>().Where(s => s.DeviceType == AutoProdDeviceType.Conveyor.ToString()).ToList();
            }
            var plc = _siemensS7Clients.SingleOrDefault(s => s.DeviceNo == device.DeviceNo);
            if (plc == null)
            {
                if (string.IsNullOrEmpty(device.ConnectAddress))
                {
                    throw new BusinessException($"设备{device.DeviceNo}IP地址为空，无法建立连接");
                }
                var ip = device.ConnectAddress.IndexOf(':') >= 0 ? device.ConnectAddress.Split(':')[0] : device.ConnectAddress;
                plc = new SiemensS7Client(device.DeviceNo);
                plc.OnClientConnected += _onConnectSuccessToPLC;
                plc.OnClientConnectFailed += _onConnectFailedToPLC;
                plc.OnDataReceived += _onReadDataSuccessToPLC;
                plc.OnDataReceivedFailed += _onReadDataFailedToPLC;
                plc.OnClientDisconnected += _onClientDisconnectedToPLC;
                plc.OnSendSuccess += _onWriteDataSuccessToPLC;
                plc.OnSendFailed += _onWriteDataFailedToPLC;
                _sendBegianConnectMsg(device.DeviceNo, device.ConnectedCount, device.ConnectAddress);
                _ = plc.ConnectAsync(ip, CpuType.S71500);
                _siemensS7Clients.Add(plc);
            }
            else
            {
                if (!plc.IsConnected)
                {
                    _sendBegianConnectMsg(device.DeviceNo, device.ConnectedCount, device.ConnectAddress);
                    _ = plc.ConnectAsync(device.ConnectAddress, CpuType.S71500);
                }
            }
        }

        /// <summary>
        /// 关闭所有设备PLC连接
        /// </summary>
        /// <param name="plcDeviceArr"></param>
        public void CloseAllPLC(List<AutoProdDeviceDto> plcDeviceArr)
        {

            if (_siemensS7Clients != null)
            {
                foreach (var device in plcDeviceArr)
                {
                    var plc = _siemensS7Clients.SingleOrDefault(s => s.DeviceNo == device.DeviceNo);
                    if (plc != null)
                    {
                        if (plc.IsConnected)
                        {
                            plc.Close();
                        }
                        plc.OnClientConnected -= _onConnectSuccessToPLC;
                        plc.OnClientConnectFailed -= _onConnectFailedToPLC;
                        plc.OnDataReceived -= _onReadDataSuccessToPLC;
                        plc.OnDataReceivedFailed -= _onReadDataFailedToPLC;
                        plc.OnClientDisconnected -= _onClientDisconnectedToPLC;
                        plc.OnSendSuccess -= _onWriteDataSuccessToPLC;
                        plc.OnSendFailed -= _onWriteDataFailedToPLC;
                    }
                }
            }
            _tasklocker = false;
            if (_rcsService.IsAgvCallbackSubscribed)
            {
                _rcsService.OnTaskResult -= _onAgvCallback;
                _rcsService.IsAgvCallbackSubscribed = false;
            }
        }

        /// <summary>
        /// 连接PLC向客户端反馈连接次数
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <param name="connectedCount"></param>
        /// <param name="address"></param>
        /// <param name="msgContent"></param>
        private void _sendBegianConnectMsg(string deviceNo, int connectedCount, string address, string msgContent = "")
        {
            var msg = new SocketMessageModel<string>
            {
                Name = deviceNo,
                Message = msgContent == "" ? $"正在尝试与{address}PLC建立连接...（第{connectedCount + 1}次尝试）" : msgContent,
                Address = address,
                Status = SocketMessageStatus.Success.ToString(),
                Type = SocketMessageType.Connect.ToString(),
                Date = DateTime.Now
            };
            _processWebSocketSend(msg);
        }

        /// <summary>
        /// PLC连接成功事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _onConnectSuccessToPLC(object? sender, PLCEventArgs e)
        {
            var plc = sender as SiemensS7Client; 
            var msg = new SocketMessageModel<string>
            {
                Name = plc?.DeviceNo,
                Message = e.Message,
                Address = e.PLCClient?.IP + ":" + e.PLCClient?.Port,
                Status = SocketMessageStatus.Success.ToString(),
                Type = SocketMessageType.Connect.ToString(),
                Date = DateTime.Now
            };
            _processWebSocketSend(msg);
            if (plc != null && !string.IsNullOrEmpty(plc.DeviceNo))
            {
                _sendTicks(plc.DeviceNo, plc);
                var curDevice = _plcDeviceInfo?.SingleOrDefault(s => s.DeviceNo == plc.DeviceNo);
                _listenReadySignal(plc.DeviceNo, curDevice?.DeviceType, plc);
                if (curDevice != null)
                {
                    curDevice.ConnectedCount = 0;
                }
            }
        }

        /// <summary>
        /// PLC连接失败事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _onConnectFailedToPLC(object? sender, PLCEventArgs e)
        {
            var msg = new SocketMessageModel<string>
            {
                Name = (sender as SiemensS7Client)?.DeviceNo,
                Message = e.Message,
                Address = e.PLCClient?.IP + ":" + e.PLCClient?.Port,
                Status = SocketMessageStatus.Failed.ToString(),
                Type = SocketMessageType.Connect.ToString(),
                Date = DateTime.Now
            };
            _processWebSocketSend(msg);
            //失败后尝试3次重新连接
            if (_plcDeviceInfo != null)
            {
                var curDevice = _plcDeviceInfo.SingleOrDefault(s => s.DeviceNo == msg.Name);
                if (curDevice != null && curDevice.ConnectedCount < 9)
                {
                    _sendBegianConnectMsg(curDevice.DeviceNo, curDevice.ConnectedCount, curDevice.ConnectAddress, "5秒后准备尝试重新建立连接");
                    var timer = new Timer();
                    timer.Interval = 5000;
                    timer.AutoReset = false;
                    timer.Elapsed += (sender, e) =>
                    {
                        curDevice.ConnectedCount++;
                        ConnectSinglePLC(curDevice);
                    };
                    timer.Start();
                }
            }
        }

        /// <summary>
        /// 接收PLC数据成功事件处理 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _onReadDataSuccessToPLC(object? sender, PLCEventArgs e)
        {
            var plc = (sender as SiemensS7Client);
            if (_autoProdDevicePlcConfigs == null)
            {
                _autoProdDevicePlcConfigs = _dbContext.GetDb().Queryable<AutoProdDevicePlcConfig>().Where(s => s.DeviceType == AutoProdDeviceType.Conveyor.ToString()).ToList();
            }
            var curPlcArgs = _autoProdDevicePlcConfigs.SingleOrDefault(s => s.DeviceNo == plc?.DeviceNo && s.DbRange + "." + s.DbOffset == e.Db && s.SignalType == "R");
            if (curPlcArgs != null && plc != null)
            {
                var msg = new SocketMessageModel<SocketMessageData>
                {
                    Name = plc?.DeviceNo,
                    Message = $"{curPlcArgs.Remark}，DB：{curPlcArgs.DbRange}.{curPlcArgs.DbOffset}，当前值：{e.Value}",
                    Address = e.PLCClient?.IP + ":" + e.PLCClient?.Port,
                    Status = SocketMessageStatus.Success.ToString(), 
                    Type = SocketMessageType.Business.ToString(),
                    Date = DateTime.Now 
                };
                if (curPlcArgs.SignalName== TransportDBConst.State_R || curPlcArgs.SignalName == TransportDBConst.RollInsideFinished_R || curPlcArgs.SignalName == TransportDBConst.RollOutsideFinished_R)
                {
                    msg.Data = new SocketMessageData
                    {
                        BusinessType = curPlcArgs.ActionType,
                        Value = e.Value
                    };
                }
                _processWebSocketSend(msg);
            }
        }

        /// <summary>
        /// 接收PLC数据失败事件处理 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _onReadDataFailedToPLC(object? sender, PLCEventArgs e)
        {
            if (_autoProdDevicePlcConfigs == null)
            {
                _autoProdDevicePlcConfigs = _dbContext.GetDb().Queryable<AutoProdDevicePlcConfig>().Where(s => s.DeviceType == AutoProdDeviceType.Conveyor.ToString()).ToList();
            }
            var plc = (sender as SiemensS7Client);
            var curPlcArgs = _autoProdDevicePlcConfigs.SingleOrDefault(s => s.DeviceNo == plc?.DeviceNo && s.DbRange + "." + s.DbOffset == e.Db && s.SignalType == "R"); 
            var msg = new SocketMessageModel<string>
            {
                Name = plc?.DeviceNo,
                Message = $"{curPlcArgs?.Remark}失败，{e.Message}",
                Address = e.PLCClient?.IP + ":" + e.PLCClient?.Port,
                Status = SocketMessageStatus.Failed.ToString(),
                Type = SocketMessageType.Read.ToString(),
                Date = DateTime.Now
            };
            _processWebSocketSend(msg);
        }

        /// <summary>
        /// 发送PLC数据成功事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _onWriteDataSuccessToPLC(object? sender, PLCEventArgs e)
        {
            if (_autoProdDevicePlcConfigs == null)
            {
                _autoProdDevicePlcConfigs = _dbContext.GetDb().Queryable<AutoProdDevicePlcConfig>().Where(s => s.DeviceType == AutoProdDeviceType.Conveyor.ToString()).ToList();
            }
            var plc = (sender as SiemensS7Client);
            var curPlcArgs = _autoProdDevicePlcConfigs.SingleOrDefault(s => s.DeviceNo == plc?.DeviceNo && s.DbRange + "." + s.DbOffset == e.Db && s.SignalType == "W");
            var msg = new SocketMessageModel<object>
            {
                Name = plc?.DeviceNo,
                Data = e.Value,
                Address = e.PLCClient?.IP + ":" + e.PLCClient?.Port,
                Status = SocketMessageStatus.Success.ToString(),
                Type = curPlcArgs.SignalName == TransportDBConst.Ticks_W ? SocketMessageType.Tick.ToString() : SocketMessageType.Write.ToString(),
                Date = DateTime.Now
            };
            _processWebSocketSend(msg);
        }

        /// <summary>
        /// 发送PLC数据失败事件处理 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _onWriteDataFailedToPLC(object? sender, PLCEventArgs e)
        {
            if (_autoProdDevicePlcConfigs == null)
            {
                _autoProdDevicePlcConfigs = _dbContext.GetDb().Queryable<AutoProdDevicePlcConfig>().Where(s => s.DeviceType == AutoProdDeviceType.Conveyor.ToString()).ToList();
            }
            var plc = (sender as SiemensS7Client);
            var curPlcArgs = _autoProdDevicePlcConfigs.SingleOrDefault(s => s.DeviceNo == plc?.DeviceNo && s.DbRange + "." + s.DbOffset == e.Db && s.SignalType == "W");
            var msg = new SocketMessageModel<string>
            {
                Name = plc?.DeviceNo,
                Message =$"{curPlcArgs?.Remark}失败，{e.Message}",
                Address = e.PLCClient?.IP + ":" + e.PLCClient?.Port,
                Status = SocketMessageStatus.Failed.ToString(),
                Type = curPlcArgs.SignalName == TransportDBConst.Ticks_W? SocketMessageType.Tick.ToString(): SocketMessageType.Write.ToString(),
                Date = DateTime.Now
            };  
            _processWebSocketSend(msg);
        }

        /// <summary>
        /// PLC连接中断事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _onClientDisconnectedToPLC(object? sender, PLCEventArgs e)
        {
            var msg = new SocketMessageModel<string>
            {
                Name = (sender as SiemensS7Client)?.DeviceNo,
                Message = e.Message,
                Address = e.PLCClient?.IP + ":" + e.PLCClient?.Port,
                Status = SocketMessageStatus.Failed.ToString(),
                Type = SocketMessageType.Connect.ToString(),
                Date = DateTime.Now
            };
            _processWebSocketSend(msg);
            //失败后尝试10次重新连接
            if (_plcDeviceInfo != null)
            {
                var curDevice = _plcDeviceInfo.SingleOrDefault(s => s.DeviceNo == msg.Name);
                if (curDevice != null && curDevice.ConnectedCount < 9)
                {
                    _sendBegianConnectMsg(curDevice.DeviceNo, curDevice.ConnectedCount, curDevice.ConnectAddress, "5秒后准备尝试重新建立连接");
                    var timer = new Timer();
                    timer.Interval = 5000;
                    timer.AutoReset = false;
                    timer.Elapsed += (sender, e) =>
                    {
                        curDevice.ConnectedCount++;
                        ConnectSinglePLC(curDevice);
                    };
                    timer.Start();
                }
            }
        }

        /// <summary>
        /// 发送心跳信号 
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <param name="plc"></param>
        private void _sendTicks(string deviceNo, SiemensS7Client plc)
        {
            if (_autoProdDevicePlcConfigs != null && _plcDeviceInfo != null)
            {
                var deviceType = _plcDeviceInfo.Single(s => s.DeviceNo == deviceNo).DeviceType; 
                var ticksWriteArgs = _autoProdDevicePlcConfigs.SingleOrDefault(s => s.DeviceNo == deviceNo && s.SignalName == TransportDBConst.Ticks_W);
                if (ticksWriteArgs != null)
                {
                    plc.WriteDurable<short>(ticksWriteArgs.DbRange + "." + ticksWriteArgs.DbOffset, 1);
                }
            }
        }

        /// <summary>
        /// 获取传送设备状态
        /// 1-允许放置物品，0-有物品，不允许放置物品
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        public int GetDeviceState(string deviceNo)
        {
            var plc = _siemensS7Clients.SingleOrDefault(s => s.DeviceNo == deviceNo);
            //var ticksWriteArgs = _autoProdDevicePlcConfigs.SingleOrDefault(s => s.DeviceNo == deviceNo && s.SignalName == TransportDBConst.Ticks_W);
            //plc.Write<short>(ticksWriteArgs.DbRange + "." + ticksWriteArgs.DbOffset, ticksWriteArgs.DeftVal);
            var signal = _autoProdDevicePlcConfigs.Single(s => s.DeviceNo == deviceNo && s.SignalName == TransportDBConst.State_R);
            var plcState = plc.Read<int>(signal.DbRange + "." + signal.DbOffset);
            return plcState;
        }

        /// <summary>
        /// 获取传送带向外滚动到位信号
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        private bool _getDeviceRollOutFinished(string deviceNo)
        {
            if (_autoProdDevicePlcConfigs != null && _siemensS7Clients != null)
            {
                var signal = _autoProdDevicePlcConfigs.Single(s => s.DeviceNo == deviceNo && s.SignalName == TransportDBConst.RollOutsideFinished_R);
                var plc = _siemensS7Clients.SingleOrDefault(s => s.DeviceNo == deviceNo);
                var plcState = plc.Read<int>(signal.DbRange + "." + signal.DbOffset);
                return plcState == int.Parse(signal.DeftVal);
            }
            return false;
        }

        /// <summary>
        /// 获取传送带向内滚动到位信号
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        private bool _getDeviceRollInFinished(string deviceNo)
        {
            if (_autoProdDevicePlcConfigs != null && _siemensS7Clients != null)
            {
                var signal = _autoProdDevicePlcConfigs.Single(s => s.DeviceNo == deviceNo && s.SignalName == TransportDBConst.RollInsideFinished_R);
                var plc = _siemensS7Clients.SingleOrDefault(s => s.DeviceNo == deviceNo);
                var plcState = plc.Read<int>(signal.DbRange + "." + signal.DbOffset);
                return plcState == int.Parse(signal.DeftVal);
            }
            return false;
        }

        /// <summary>
        /// 向传送带发送向外滚动信号,同时监测传送带向往滚动到位信号
        /// </summary>
        /// <param name="deviceNo"></param>
        public void SendDeviceRollOut(string deviceNo)
        {
            if (_autoProdDevicePlcConfigs != null && _siemensS7Clients != null)
            {
                var signal = _autoProdDevicePlcConfigs.Single(s => s.DeviceNo == deviceNo && s.SignalName == TransportDBConst.RollOutside_W);
                var plc = _siemensS7Clients.SingleOrDefault(s => s.DeviceNo == deviceNo);
                plc.Write<short>(signal.DbRange + "." + signal.DbOffset, int.Parse(signal.DeftVal));  
            }
        }

        /// <summary>
        /// 向传送带发送向内滚动信号，同时查询当前传送待上的任务
        /// 如果当前任务类型是OutInvstorage，向内滚动的同时呼叫AGV执行还料箱任务
        /// </summary>
        /// <param name="deviceNo"></param>
        public void SendDeviceRollIn(string deviceNo)
        {
            var signal = _autoProdDevicePlcConfigs?.Single(s => s.DeviceNo == deviceNo && s.SignalName == TransportDBConst.RollInside_W);
            var plc = _siemensS7Clients?.SingleOrDefault(s => s.DeviceNo == deviceNo);
            plc.Write<short>(signal.DbRange + "." + signal.DbOffset, int.Parse(signal.DeftVal));
            var dbClient = _dbContext.GetDb();
            var curExecuteTask = dbClient.Queryable<AutoProdTaskTracking>().Single(s =>s.DestinationDeviceNo== deviceNo && s.IsExecuting && s.TaskStatus==AGVTaskStatus.End.ToString()&&s.ReturnTaskStatus==AGVTaskStatus.Await.ToString());
            if (curExecuteTask != null)
            {
                ExecuteTransTask(curExecuteTask.TaskId);
            } 
        }

        /// <summary>
        /// 向传送带发送复位信号
        /// </summary>
        /// <param name="deviceNo"></param>
        private void _sendDeviceReset(string deviceNo)
        {
            if (_autoProdDevicePlcConfigs != null && _siemensS7Clients != null)
            {
                var signal = _autoProdDevicePlcConfigs.Single(s => s.DeviceNo == deviceNo && s.SignalName == TransportDBConst.Reset_W);
                var plc = _siemensS7Clients.SingleOrDefault(s => s.DeviceNo == deviceNo);
                plc.Write<short>(signal.DbRange + "." + signal.DbOffset, int.Parse(signal.DeftVal));
            }
        }

        /// <summary>
        /// 各种就绪信号监测 
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <param name="plc"></param>
        private void _listenReadySignal(string deviceNo, string deviceType, SiemensS7Client plc)
        {
            if (_autoProdDevicePlcConfigs != null && _plcDeviceInfo != null)
            {
                GetDeviceState(deviceNo); 
                var signal = _autoProdDevicePlcConfigs.Single(s => s.DeviceNo == deviceNo && s.SignalName == TransportDBConst.RollOutsideFinished_R);
                plc.ReadDurable(signal.DbRange + "." + signal.DbOffset);  
            }
        }

        /// <summary>
        /// 当系统生成入库单或出库单时，同时创建关联的AGV运送任务，模式默认为Single单任务模式
        /// 1.单任务模式：选择AGV单任务模板，2个地标码，创建领料出库任务后还需要自动创建入库还料任务
        /// 2.多任务模式：选择AGV多任务模板，4个地标码，一个任务完成出库和入库还料任务
        /// </summary>
        public void CreateTransTask(AutoTransTaskInput data)
        {
            var dbClent = _dbContext.GetDb();
            var goodsClassifyDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(data.GoodsClassifyGroup);
            var agvItems = dbClent.Queryable<AutoProdDeviceAGV>().Where(w => w.AGVType == AGVType.CTU.ToString()).ToList();
            var freeItem = agvItems.FirstOrDefault(w => w.Status == AGVStatus.Free.ToString());
            var curUseAgv = freeItem == null ? agvItems.First() : freeItem;
            var conveyor = dbClent.Queryable<AutoProdDevice>().Where(w => w.DeviceType == AutoProdDeviceType.Conveyor.ToString()).First();
            var conveyorBins = dbClent.Queryable<AutoProdDeviceWarehouse>().Where(w => w.DeviceId == conveyor.DeviceId && w.Sort == 1).Single();
            var taskTracking = new List<AutoProdTaskTracking>();
            if (data.TaskType == AutoTransTaskType.OutStorage.ToString())
            { 
                //查询出库单记录 
                var outInvOrder = dbClent.Queryable<InvOutStorage>().Where(w => w.OutStorageType == data.BusinessType && w.OrderNo == data.OrderNo && w.Status == OutStorageStatus.WaitOutStorage.ToString()).Single();
                if (outInvOrder != null)
                {
                    var outInvOrderDetails = dbClent.Queryable<InvOutStorageDetail>().Where(w => w.OrderNo == outInvOrder.OrderNo).ToList();
                    if(outInvOrderDetails!=null && outInvOrderDetails.Count > 0)
                    {
                        //查询满足领用数量的货位（可能存在一个货位的存量无法满足当前领用的数量需求，需要从多个货位领取）
                        var goodsIdArr = outInvOrderDetails.Select(s => s.GoodsId).ToList();
                        var stockInfo = dbClent.Queryable<InvStorageWarehouseDetail>().Where(w => goodsIdArr.Contains(w.GoodsId)).ToList();
                        var newOutInvOrderDetails = new List<InvOutStorageDetail>();
                        foreach (var detail in outInvOrderDetails)
                        {
                            var curStockTotal = stockInfo.Where(s => s.GoodsId == detail.GoodsId).Sum(s => s.Stock);
                            if (curStockTotal < detail.Quantity)
                            {
                                if (data.IsRequirementForRemaining)
                                {
                                    //库存总量不满足当前领用数量时，按库存量从大到小依次领用并扣减 
                                    var curBinStock = stockInfo.Where(s => s.GoodsId == detail.GoodsId && s.UnitId == detail.UnitId).OrderByDescending(s => s.Stock).ToList();
                                    foreach (var binStock in curBinStock)
                                    {
                                        //重新生成该物料的出库单明细
                                        newOutInvOrderDetails.Add(new InvOutStorageDetail
                                        {
                                            OrderNo = detail.OrderNo,
                                            GoodsId = detail.GoodsId,
                                            GoodsName = detail.GoodsName,
                                            Quantity = binStock.Stock,
                                            ActualQuantity = binStock.Stock,
                                            UnitId = detail.UnitId,
                                            WarehouseId = binStock.WarehouseId,
                                            ShelfId = binStock.ShelfId,
                                            BinId = binStock.BinId,
                                            WorkbinId = binStock.WorkbinId,
                                            WorkbinCellId = binStock.WorkbinCellId
                                        });
                                    }
                                }
                            }
                            else
                            {
                                //库位存总量满足当前领用数量时，按库存量从大到小依次领用并扣减
                                var remainderQty = detail.Quantity;
                                var curBinStock = stockInfo.Where(s => s.GoodsId == detail.GoodsId && s.UnitId == detail.UnitId).OrderByDescending(s => s.Stock).ToList();
                                foreach (var binStock in curBinStock)
                                {
                                    var qty = (binStock.Stock >= remainderQty) ? remainderQty : binStock.Stock;
                                    remainderQty -= qty;
                                    //重新生成该物料的出库单明细
                                    newOutInvOrderDetails.Add(new InvOutStorageDetail
                                    {
                                        OrderNo = detail.OrderNo,
                                        GoodsId = detail.GoodsId,
                                        GoodsName = detail.GoodsName,
                                        Quantity = qty,
                                        ActualQuantity = qty,
                                        UnitId = detail.UnitId,
                                        WarehouseId = binStock.WarehouseId,
                                        ShelfId = binStock.ShelfId,
                                        BinId = binStock.BinId,
                                        WorkbinId = binStock.WorkbinId,
                                        WorkbinCellId = binStock.WorkbinCellId
                                    });
                                    if (remainderQty == 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if (newOutInvOrderDetails.Count == 0)
                        {
                            return;
                        }
                        outInvOrder.WarehouseId = newOutInvOrderDetails[0].WarehouseId;
                        //按货位整合新的出库明细记录，创建AGV任务跟踪信息  
                        var warehouseBinGroups = newOutInvOrderDetails.Select(s => s.BinId).Distinct().ToList(); 
                        var warehouseBinGroupsInfo = dbClent.Queryable<InvBin>().Where(w => warehouseBinGroups.Contains(w.BinId)).ToList();
                        var workbinCellGroups = newOutInvOrderDetails.Where(s => s.WorkbinCellId > 0).Select(s => s.WorkbinCellId).Distinct().ToList();
                        var workbinCellGroupsInfo= dbClent.Queryable<InvWorkbinCell>().Where(w => workbinCellGroups.Contains(w.CellId)).ToList();
                        foreach (var warehouseBin in warehouseBinGroups)
                        {
                            //创建取料箱任务
                            var curBin = warehouseBinGroupsInfo.Single(s => s.BinId == warehouseBin);
                            string workbinCellStr = "";
                            if(workbinCellGroupsInfo!=null&& workbinCellGroupsInfo.Count > 0)
                            {
                                var curWorkbinCell = workbinCellGroupsInfo.Where(s => s.BinId == warehouseBin).Select(s => s.CellNo).ToList();
                                workbinCellStr = string.Join(',', curWorkbinCell);
                            } 
                            var reqCode = Guid.NewGuid().ToString("N").ToUpper();
                            taskTracking.Add(new AutoProdTaskTracking
                            {
                                AGVReqCode = reqCode,
                                TaskCode = reqCode,
                                TaskModel = data.TaskModel,
                                OrderNo = outInvOrder.OrderNo,
                                GoodsClassifyGroup = data.GoodsClassifyGroup,
                                BusinessType = outInvOrder.OutStorageType,
                                GoodsInfo = JsonConvert.SerializeObject(newOutInvOrderDetails.Where(w => w.BinId == warehouseBin)),
                                TaskType = AutoTransTaskType.OutStorage.ToString(),
                                TaskStatus = AGVTaskStatus.Await.ToString(),
                                ReturnTaskStatus = AGVTaskStatus.Await.ToString(),
                                ActionType = AGVActionType.Deliver.ToString(),
                                StartingDeviceNo = curBin.ShelfId,
                                StartingDeviceType = StorageUnitType.Warehouse.ToString(),
                                StartingAGVPositionNo = curBin.AGVNo,
                                StartingBinNo = string.IsNullOrEmpty(workbinCellStr)?curBin.BinNo: workbinCellStr,
                                StartingBinRank = curBin.Rank,
                                DestinationDeviceNo = conveyor.DeviceNo,
                                DestinationDeviceType = conveyor.DeviceType,
                                DestinationAGVPositionNo = conveyorBins.AGVBinCode_Delivery,
                                DestinationBinNo = conveyorBins.BinNo,
                                DestinationBinRank = conveyorBins.Sort,
                                TaskCreateTime = DateTime.Now,
                                OperatorId = data.OperatorId,
                                OperatorName = data.OperatorName
                            }); 
                        }
                        //更新出库明细记录
                        dbClent.Deleteable(outInvOrderDetails).AddQueue();
                        dbClent.Insertable(newOutInvOrderDetails).AddQueue();
                        dbClent.Updateable(outInvOrder).AddQueue();
                        //写入AGV调度信息
                        dbClent.Deleteable<AutoProdTaskTracking>(d => d.OrderNo == outInvOrder.OrderNo && d.BusinessType == outInvOrder.OutStorageType).AddQueue();
                        dbClent.Insertable(taskTracking).AddQueue();
                        dbClent.SaveQueues();
                    }
                    else
                    {
                        dbClent.Deleteable<AutoProdTaskTracking>(d => d.OrderNo == outInvOrder.OrderNo && d.BusinessType == outInvOrder.OutStorageType).AddQueue();
                        dbClent.SaveQueues();
                    }
                }
                else
                {
                    dbClent.Deleteable<AutoProdTaskTracking>(d => d.OrderNo == data.OrderNo && d.BusinessType == data.BusinessType).AddQueue();
                    dbClent.SaveQueues();
                }
            }
            else
            {
                //处理入库任务 
                var exist = dbClent.Queryable<AutoProdTaskTracking>().Any(w => w.GoodsClassifyGroup == data.GoodsClassifyGroup && w.OrderNo == data.OrderNo);
                if (exist)
                {
                    return;
                }
                var inInvOrder = dbClent.Queryable<InvInStorage>().Where(w => w.InStorageType == data.BusinessType && w.OrderNo == data.OrderNo && w.Status == InStorageStatus.WaitInStorage.ToString()).Single();
                if (inInvOrder != null)
                { 
                    var inInvOrderDetails = dbClent.Queryable<InvInStorageDetail>().Where(w => w.OrderNo == inInvOrder.OrderNo).ToList();
                    if(inInvOrderDetails!=null && inInvOrderDetails.Count > 0)
                    {
                        var warehouseBinGroups = inInvOrderDetails.Select(s => s.BinId).Distinct().ToList();
                        var warehouseBinGroupsInfo = dbClent.Queryable<InvBin>().Where(w => warehouseBinGroups.Contains(w.BinId)).ToList();
                        var workbinCellGroups = inInvOrderDetails.Where(s => s.WorkbinCellId > 0).Select(s => s.WorkbinCellId).Distinct().ToList();
                        var workbinCellGroupsInfo = dbClent.Queryable<InvWorkbinCell>().Where(w => workbinCellGroups.Contains(w.CellId)).ToList();
                        foreach (var warehouseBin in warehouseBinGroups)
                        {
                            //创建取料箱任务
                            var curBin = warehouseBinGroupsInfo.Single(s => s.BinId == warehouseBin);
                            string workbinCellStr = "";
                            if (workbinCellGroupsInfo != null && workbinCellGroupsInfo.Count > 0)
                            {
                                var curWorkbinCell = workbinCellGroupsInfo.Where(s => s.BinId == warehouseBin).Select(s => s.CellNo).ToList();
                                workbinCellStr = string.Join(',', curWorkbinCell);
                            }
                            var reqCode = Guid.NewGuid().ToString("N").ToUpper();
                            taskTracking.Add(new AutoProdTaskTracking
                            {
                                AGVReqCode = reqCode,
                                TaskCode = reqCode,
                                TaskModel = data.TaskModel,
                                OrderNo = inInvOrder.OrderNo,
                                GoodsClassifyGroup = data.GoodsClassifyGroup,
                                BusinessType = inInvOrder.InStorageType,
                                GoodsInfo = JsonConvert.SerializeObject(inInvOrderDetails.Where(w => w.BinId == warehouseBin)),
                                TaskType = AutoTransTaskType.InStorage.ToString(),
                                TaskStatus = AGVTaskStatus.Await.ToString(),
                                ReturnTaskStatus = AGVTaskStatus.Await.ToString(),
                                ActionType = AGVActionType.Deliver.ToString(),
                                StartingDeviceNo = curBin.ShelfId,
                                StartingDeviceType = StorageUnitType.Warehouse.ToString(),
                                StartingAGVPositionNo = curBin.AGVNo,
                                StartingBinNo = string.IsNullOrEmpty(workbinCellStr) ? curBin.BinNo : workbinCellStr,
                                StartingBinRank = curBin.Rank,
                                DestinationDeviceNo = conveyor.DeviceNo,
                                DestinationDeviceType = conveyor.DeviceType,
                                DestinationAGVPositionNo = conveyorBins.AGVBinCode_Delivery,
                                DestinationBinNo = conveyorBins.BinNo,
                                DestinationBinRank = conveyorBins.Sort,
                                TaskCreateTime = DateTime.Now,
                                OperatorId = data.OperatorId,
                                OperatorName = data.OperatorName
                            }); 
                        }
                        //写入AGV调度信息
                        dbClent.Deleteable<AutoProdTaskTracking>(d => d.OrderNo == inInvOrder.OrderNo && d.BusinessType == inInvOrder.InStorageType).AddQueue();
                        dbClent.Insertable(taskTracking).AddQueue();
                        dbClent.SaveQueues();
                    }
                    else
                    {
                        dbClent.Deleteable<AutoProdTaskTracking>(d => d.OrderNo == inInvOrder.OrderNo && d.BusinessType == inInvOrder.InStorageType).AddQueue(); 
                        dbClent.SaveQueues();
                    }
                }
                else
                {
                    dbClent.Deleteable<AutoProdTaskTracking>(d => d.OrderNo == data.OrderNo && d.BusinessType == data.BusinessType).AddQueue();
                    dbClent.SaveQueues();
                }
            } 
        }
         
        /// <summary>
        /// 查询待执行和正在执行的运送任务
        /// </summary>
        /// <returns></returns>
        public List<AutoProdTaskTrackingDto> GetPendingExecTransTask(string goodsClassifyGroup,string userId)
        {
            var dbClent = _dbContext.GetDb();
            var tsk= dbClent.Queryable<AutoProdTaskTracking>().Where(w =>w.GoodsClassifyGroup== goodsClassifyGroup &&w.OperatorId== userId 
            && (w.TaskStatus == AGVTaskStatus.Await.ToString() || w.IsExecuting)).Select<AutoProdTaskTrackingDto>().ToList();
            tsk.ForEach(f =>
            {
                if(f.IsReturn)
                {
                    f.StatusDesc = "送回料箱-" + EnumHelper.GetDescFromEnumVal<AGVTaskStatus>(f.ReturnTaskStatus);
                }
                else
                {
                    f.StatusDesc = "取出料箱-" + EnumHelper.GetDescFromEnumVal<AGVTaskStatus>(f.TaskStatus);
                }
            });
            return tsk.OrderBy(o => o.IsExecuting).ThenByDescending(o => o.TaskId).ToList();
        } 
         

        /// <summary>
        /// 根据任务ID执行AGV调度 ,如果是单任务模式，且状态为已放下料箱，则将起始地和目的地对换
        /// </summary>
        /// <param name="taskId"></param>
        /// <exception cref="BusinessException"></exception>
        public void ExecuteTransTask(int taskId)
        {
            if (!_tasklocker)
            {
                _tasklocker = true;
                if (_autoProdDevicePlcConfigs == null)
                {
                    _autoProdDevicePlcConfigs = _dbContext.GetDb().Queryable<AutoProdDevicePlcConfig>().Where(s => s.DeviceType == AutoProdDeviceType.Conveyor.ToString()).ToList();
                } 
                try
                {
                    var dbClent = _dbContext.GetDb();
                    var curTask = dbClent.Queryable<AutoProdTaskTracking>().Single(s => s.TaskId == taskId);
                    var goodsClassifyDesc = EnumHelper.GetDescFromEnumVal<BaseTypeGroup>(curTask.GoodsClassifyGroup);
                    //判断当前传送带设备是否存在未完成的任务
                    var isNotFinishedTask = dbClent.Queryable<AutoProdTaskTracking>().Where(w => w.DestinationDeviceNo == curTask.DestinationDeviceNo &&w.IsExecuting&&w.TaskId!=taskId).Any();
                    if (isNotFinishedTask)
                    {
                        _tasklocker = false;
                        throw new BusinessException($"当前存在未完成的AGV送货任务，请稍后再试...");
                    }
                    //判断当前任务是否允许执行
                    if (curTask.TaskStatus == AGVTaskStatus.Cancel.ToString())
                    {
                        _tasklocker = false;
                        throw new BusinessException($"当前任务已取消不再继续执行");
                    }
                    else if (curTask.IsExecuting && curTask.TaskStatus != AGVTaskStatus.End.ToString() && curTask.ReturnTaskStatus != AGVTaskStatus.Await.ToString())
                    {
                        _tasklocker = false;
                        throw new BusinessException($"当前任务正在执行中，请勿重复执行");
                    }
                    //复位传送带PLC信号
                    _sendDeviceReset(curTask.DestinationDeviceNo);
                    //判断传送设备的状态  
                    var plcState = GetDeviceState(curTask.DestinationDeviceNo);
                    if ((curTask.IsExecuting && plcState == 0)||(!curTask.IsExecuting && plcState == 1))
                    {
                        var agvItems = dbClent.Queryable<AutoProdDeviceAGV>().Where(w => w.AGVType == AGVType.CTU.ToString()).ToList();
                        var freeItem = agvItems.FirstOrDefault(w => w.Status == AGVStatus.Free.ToString());
                        var curUseAgv = freeItem == null ? agvItems.First() : freeItem;
                        //单任务模式，只需要给到1组起始地和目的地的仓位码 
                        var positions = new RCS_AGV_Position[] { };
                        if (curTask.TaskModel == AGVTaskModel.TransportSingle.ToString())
                        {
                            positions = new RCS_AGV_Position[] {
                                                new RCS_AGV_Position { type = curUseAgv.PositionCodeType, positionCode = curTask.StartingAGVPositionNo },
                                                new RCS_AGV_Position { type = curUseAgv.PositionCodeType, positionCode = curTask.DestinationAGVPositionNo }
                             };
                            if (curTask.IsReturn)
                            {
                                positions = new RCS_AGV_Position[] {
                                                new RCS_AGV_Position { type = curUseAgv.PositionCodeType, positionCode = curTask.DestinationAGVPositionNo },
                                                new RCS_AGV_Position { type = curUseAgv.PositionCodeType, positionCode = curTask.StartingAGVPositionNo }
                             };
                            }
                        }
                        //多任务模式，需要2组起始地和目的地的仓位码
                        else
                        {
                            positions = new RCS_AGV_Position[] {
                                                new RCS_AGV_Position { type = curUseAgv.PositionCodeType, positionCode = curTask.StartingAGVPositionNo },
                                                new RCS_AGV_Position { type = curUseAgv.PositionCodeType, positionCode = curTask.DestinationAGVPositionNo },
                                                new RCS_AGV_Position { type = curUseAgv.PositionCodeType, positionCode = curTask.DestinationAGVPositionNo },
                                                new RCS_AGV_Position { type = curUseAgv.PositionCodeType, positionCode = curTask.StartingAGVPositionNo }
                            }; 
                        }
                        var reqCode = Guid.NewGuid().ToString("N").ToUpper();
                        curTask.AGVReqCode = reqCode;
                        curTask.TaskCode= reqCode;
                        var agvActionRes = _rcsService.ScheduleAgvTaskCollection(curTask.AGVReqCode, curTask.TaskModel, curUseAgv.CtnrType, curTask.TaskCode, positions);
                        bool isActionSuccess = true;
                        string errMsg = "";
                        if (agvActionRes != null)
                        {
                            if (agvActionRes.code != "0")
                            {
                                isActionSuccess = false;
                                errMsg = agvActionRes.message; 
                            }
                        }
                        else
                        {
                            isActionSuccess = false;
                            errMsg = "系统调度AGV失败，请求RCS出错，详细请检查日志"; 
                        }
                        if (isActionSuccess)
                        { 
                            _processWebSocketSend(new SocketMessageModel<bool>
                            {
                                Name = $"AGV调度：{goodsClassifyDesc}领用出库",
                                Type = SocketMessageType.Business.ToString(),
                                Date = DateTime.Now,
                                Status = SocketMessageStatus.Success.ToString(),
                                Message = $"已创建AGV调度任务，即将开始执行..."
                            });
                            curTask.IsExecuting = true;
                            curTask.TaskStartTime = DateTime.Now.ToString();
                            dbClent.Updateable(curTask).AddQueue();
                            var transporDevice = dbClent.Queryable<AutoProdDevice>().Single(s => s.DeviceNo == curTask.DestinationDeviceNo);
                            transporDevice.IsUsing = true;
                            transporDevice.AGVTaskId = curTask.TaskId;
                            dbClent.Updateable(transporDevice).AddQueue();
                            dbClent.SaveQueues();
                            _tasklocker = false;
                        }
                        else
                        { 
                            _tasklocker = false;
                            throw new BusinessException(errMsg);
                        } 
                    }
                    else
                    { 
                        _tasklocker = false;
                        throw new BusinessException($"传送装置状态未就绪，当前值{plcState}，请检查设备上是否存在其他物品");
                    } 
                }
                catch (Exception ex)
                {
                    _tasklocker = false;
                    throw new BusinessException($"执行AGV运输任务出错：{ex.Message}");
                }
            } 
        }
         

        /// <summary>
        /// AGV任务通知回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void _onAgvCallback(object? sender, RCSCallbackArgs args)
        {
            try
            {
                var dbClient = _dbContext.GetDb();
                var trackingInfo = dbClient.Queryable<AutoProdTaskTracking>().Single(s => s.TaskCode == args.taskCode);
                if (trackingInfo == null)
                { 
                    _processWebSocketSend(new SocketMessageModel<string>
                    {
                        Name = "AGV任务通知处理异常",
                        Message = $"AGV任务通知处理中未查询到Tracking任务信息，任务码：{args.taskCode}",
                        Status = SocketMessageStatus.Failed.ToString(),
                        Type = SocketMessageType.AGVCallback.ToString(),
                        Date = DateTime.Now
                    });
                    return;
                }
                if (trackingInfo.TaskModel == AGVTaskModel.TransportMultipl.ToString())
                {
                    //多任务模式
                    _multipleModelHandler(dbClient, trackingInfo, args);
                }
                else
                {
                    //单任务模式
                    _singleModelHandler(dbClient, trackingInfo, args);
                } 
            }
            catch(Exception e)
            { 
                _processWebSocketSend(new SocketMessageModel<string>
                {
                    Name = "AGV任务通知处理异常",
                    Message = $"任务动作：{args.method}，异常信息：{e.Message}",
                    Status = SocketMessageStatus.Failed.ToString(),
                    Type = SocketMessageType.AGVCallback.ToString(),
                    Date = DateTime.Now
                });
            }
        }


        /// <summary>
        /// AGV多任务模式回调处理
        /// </summary>
        private void _multipleModelHandler(SqlSugarClient dbClient, AutoProdTaskTracking trackingInfo, RCSCallbackArgs args)
        {
            var socketMsg = new SocketMessageModel<SocketMessageData>
            {
                Name = EnumHelper.GetDescFromEnumVal<AutoTransTaskType>(trackingInfo.TaskType),
                Type = SocketMessageType.AGVCallback.ToString(),
                Date = DateTime.Now,
                Data = new SocketMessageData { BusinessType=args.method, TaskStatus = EnumHelper.GetDescFromEnumVal<AGVTaskStatus>(args.method) }
            }; 
            //AGV启动
            if (args.method == AGVTaskStatus.Start.ToString())
            { 
                trackingInfo.TaskStatus = args.method;
                dbClient.Updateable(trackingInfo).AddQueue();
                dbClient.SaveQueues();
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                socketMsg.Message = "AGV正在前往指定货位，请稍后...";
                _processWebSocketSend(socketMsg);
            }
            //AGV到达起始仓
            else if (args.method == AGVTaskStatus.Arrive.ToString())
            {
                trackingInfo.TaskStatus = args.method;
                dbClient.Updateable(trackingInfo).ExecuteCommand();
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                socketMsg.Message = $"AGV已到达存储位：{trackingInfo.StartingBinNo}";
                socketMsg.Data.AGVDockDeviceNo = trackingInfo.StartingDeviceNo;
                _processWebSocketSend(socketMsg);
            }
            //AGV到达目标仓（传送带），等待放下料箱
            else if (args.method == AGVTaskStatus.WaitPutdown.ToString())
            {
                trackingInfo.TaskStatus = args.method;
                dbClient.Updateable(trackingInfo).ExecuteCommand();
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                socketMsg.Message = $"AGV已到达传送位：{trackingInfo.DestinationBinNo}，申请放下料箱...";
                socketMsg.Data.AGVDockDeviceNo = trackingInfo.DestinationDeviceNo;
                _processWebSocketSend(socketMsg);
                //检查传送带的状态
                var deviceStateIsEmpty = false;
                while (!deviceStateIsEmpty)
                {
                    deviceStateIsEmpty = GetDeviceState(trackingInfo.DestinationDeviceNo) == 1;
                    Thread.Sleep(1500);
                }
                if (deviceStateIsEmpty)
                {
                    //调用AGV接口，启动AGV放下料箱
                    var applyRes = _rcsService.AgvAppllyPass(args.taskCode, 2);
                    if (applyRes.code != "0")
                    {
                        socketMsg.Status = SocketMessageStatus.Failed.ToString();
                        socketMsg.Message = $"AGV申请放料箱失败，${applyRes.message}";
                        _processWebSocketSend(socketMsg);
                    }
                    else
                    {
                        //监测传送带是否存在料箱(料箱是否已放下)，如果存在则启动传送带向外滚动
                        var deviceStateIsFull = false;
                        double awaitTime = 0;
                        while (!deviceStateIsFull)
                        {
                            awaitTime += 1.5;
                            deviceStateIsFull = GetDeviceState(trackingInfo.DestinationDeviceNo) == 0;
                            Thread.Sleep(1500);
                            if (awaitTime >= 10)
                            {
                                deviceStateIsFull = true;
                                break;
                            } 
                        }
                        if (deviceStateIsFull)
                        {
                            //发送向往滚动信号，同时监测传送带向往滚动到位信号
                            trackingInfo.TaskStatus = AGVTaskStatus.Putdown.ToString();
                            SendDeviceRollOut(trackingInfo.DestinationDeviceNo);
                            socketMsg.Data.BusinessType = trackingInfo.TaskStatus;
                            socketMsg.Status = SocketMessageStatus.Success.ToString();
                            socketMsg.Message = $"AGV申请放料箱通过，已启动传送带向外滚动";
                            _processWebSocketSend(socketMsg); 
                        }
                    }

                }
            }
            //AGV等待收回料箱（用户取完料后，点击退还后传送带向内滚动）
            else if (args.method == AGVTaskStatus.WaitePickup.ToString())
            {
                trackingInfo.TaskStatus = args.method;
                dbClient.Updateable(trackingInfo).ExecuteCommand();
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                socketMsg.Message = $"AGV申请收回当前料箱..."; 
                _processWebSocketSend(socketMsg);
                //监测传送带向内滚动到位信号
                var deviceStateIsRollInFinished = false;
                double awaitTime = 0;
                while (!deviceStateIsRollInFinished)
                {
                    deviceStateIsRollInFinished = _getDeviceRollInFinished(trackingInfo.DestinationDeviceNo);
                    Thread.Sleep(1500);
                    awaitTime += 1.5;
                    if (awaitTime >= 60)
                    {
                        deviceStateIsRollInFinished = true;
                        break;
                    }
                }
                if (deviceStateIsRollInFinished)
                {
                    socketMsg.Status = SocketMessageStatus.Success.ToString();
                    socketMsg.Message = $"传送带已将物料收回到仓内";
                    _processWebSocketSend(socketMsg);
                    //调用AGV接口，启动AGV收回当前料箱
                    var applyRes = _rcsService.AgvAppllyPass(args.taskCode, 1);
                    if (applyRes.code != "0")
                    {
                        socketMsg.Status = SocketMessageStatus.Failed.ToString();
                        socketMsg.Message = $"AGV申请取回料箱失败，${applyRes.message}";
                        _processWebSocketSend(socketMsg);
                    }
                    else
                    {
                        socketMsg.Status = SocketMessageStatus.Success.ToString();
                        socketMsg.Message = $"AGV已收回当前料箱";
                        _processWebSocketSend(socketMsg);

                        //复位传送带
                        _sendDeviceReset(trackingInfo.DestinationDeviceNo);
                    }
                    
                }
            }
            //AGV任务已完成
            else if (args.method == AGVTaskStatus.End.ToString())
            {
                //复位传送带
                _sendDeviceReset(trackingInfo.DestinationDeviceNo);
                trackingInfo.TaskEndTime = DateTime.Now.ToString();
                trackingInfo.TaskStatus = args.method;
                trackingInfo.IsExecuting = false;
                dbClient.Updateable(trackingInfo).AddQueue();
                var transporDevice = dbClient.Queryable<AutoProdDevice>().Single(s => s.DeviceNo == trackingInfo.DestinationDeviceNo);
                transporDevice.IsUsing = false;
                transporDevice.AGVTaskId =0;
                dbClient.Updateable(transporDevice).AddQueue();
                dbClient.SaveQueues();
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                socketMsg.Message = "AGV本次任务已完成!";
                _processWebSocketSend(socketMsg);
            }
            //AGV任务已取消
            else if (args.method == AGVTaskStatus.Cancel.ToString())
            {
                trackingInfo.TaskStatus = args.method;
                trackingInfo.IsExecuting = false;
                dbClient.Updateable(trackingInfo).AddQueue();
                var transporDevice = dbClient.Queryable<AutoProdDevice>().Single(s => s.DeviceNo == trackingInfo.DestinationDeviceNo);
                transporDevice.IsUsing = false;
                transporDevice.AGVTaskId = 0;
                dbClient.Updateable(transporDevice).AddQueue();
                dbClient.SaveQueues();
                socketMsg.Status = SocketMessageStatus.Failed.ToString();
                socketMsg.Message = "AGV本次任务已被取消!";
                _processWebSocketSend(socketMsg);
            }
        }


        /// <summary>
        /// AGV单任务模式回调处理
        /// </summary>
        private void _singleModelHandler(SqlSugarClient dbClient, AutoProdTaskTracking trackingInfo, RCSCallbackArgs args)
        {
            var socketMsg = new SocketMessageModel<SocketMessageData>
            {
                Name = EnumHelper.GetDescFromEnumVal<AutoTransTaskType>(trackingInfo.TaskType),
                Type = SocketMessageType.AGVCallback.ToString(),
                Date = DateTime.Now,
                Data = new SocketMessageData { BusinessType = args.method }
            };
            if (trackingInfo.IsReturn)
            {
                trackingInfo.ReturnTaskStatus = args.method;
                socketMsg.Data.TaskStatus = "送回料箱-" + EnumHelper.GetDescFromEnumVal<AGVTaskStatus>(args.method);
            }
            else
            {
                trackingInfo.TaskStatus = args.method;
                socketMsg.Data.TaskStatus = "取出料箱-" + EnumHelper.GetDescFromEnumVal<AGVTaskStatus>(args.method);
            }
            //AGV启动
            if (args.method == AGVTaskStatus.Start.ToString())
            {   
                var transporDevice = dbClient.Queryable<AutoProdDevice>().Single(s => s.DeviceNo == trackingInfo.DestinationDeviceNo);
                transporDevice.IsUsing = true;
                transporDevice.AGVTaskId = trackingInfo.TaskId;
                dbClient.Updateable(transporDevice).AddQueue(); 
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                socketMsg.Message = "AGV正在前往指定货位，请稍后..."; 
                _processWebSocketSend(socketMsg);
            }
            //AGV离开起始仓
            else if (args.method == AGVTaskStatus.Arrive.ToString())
            { 
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                if (trackingInfo.IsReturn)
                {
                    socketMsg.Message = $"AGV已到达：{trackingInfo.DestinationBinNo}";
                    socketMsg.Data.AGVDockDeviceNo = trackingInfo.DestinationDeviceNo;
                    //复位传送带PLC信号
                    _sendDeviceReset(trackingInfo.DestinationDeviceNo);
                }
                else
                {
                    socketMsg.Message = $"AGV已到达：{trackingInfo.StartingBinNo}";
                    socketMsg.Data.AGVDockDeviceNo = trackingInfo.StartingDeviceNo;
                }  
                _processWebSocketSend(socketMsg);
            }
            //AGV取料箱申请（两种情况：1.AGV取货架上的料箱，2.AGV取传送带上的料箱）
            else if (args.method == AGVTaskStatus.WaitePickup.ToString())
            { 
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                socketMsg.Message = $"AGV正在申请取料箱...";
                if (trackingInfo.IsReturn)
                {
                    socketMsg.Data.AGVDockDeviceNo = trackingInfo.DestinationDeviceNo; 
                }
                else
                {
                    socketMsg.Data.AGVDockDeviceNo = trackingInfo.StartingDeviceNo;
                }
                _processWebSocketSend(socketMsg);

                //调用AGV接口，直接允许取料箱
                var applyRes = _rcsService.AgvAppllyPass(args.taskCode, 1);
                if (applyRes.code != "0")
                {
                    socketMsg.Status = SocketMessageStatus.Failed.ToString();
                    socketMsg.Message = $"AGV申请取料箱失败，${applyRes.message}";
                    _processWebSocketSend(socketMsg);
                }
                else
                {
                    socketMsg.Status = SocketMessageStatus.Success.ToString();
                    socketMsg.Message = $"AGV申请取料箱通过";
                    _processWebSocketSend(socketMsg);
                }
            }
            //AGV放料箱申请（两种情况：1.AGV放料箱到传送带上，2.AGV放料箱到货架上）
            else if (args.method == AGVTaskStatus.WaitPutdown.ToString())
            { 
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                if (trackingInfo.IsReturn)
                {
                    socketMsg.Message = $"AGV已到达：{trackingInfo.StartingDeviceNo}，申请放下料箱...";
                    socketMsg.Data.AGVDockDeviceNo = trackingInfo.StartingDeviceNo;
                }
                else
                {
                    socketMsg.Message = $"AGV已到达：{trackingInfo.DestinationDeviceNo}，申请放下料箱...";
                    socketMsg.Data.AGVDockDeviceNo = trackingInfo.DestinationDeviceNo; 
                } 
                _processWebSocketSend(socketMsg); 
                //调用AGV接口，直接允许放下料箱
                var applyRes = _rcsService.AgvAppllyPass(args.taskCode, 2);
                if (applyRes.code != "0")
                {
                    socketMsg.Status = SocketMessageStatus.Failed.ToString();
                    socketMsg.Message = $"AGV申请放料箱失败，${applyRes.message}";
                    _processWebSocketSend(socketMsg);
                }
                else
                { 
                    //如果是放料箱到传送带上，则需要更新传送带状态,如果是放在货架上，则重置传送带状态
                    var transporDevice = dbClient.Queryable<AutoProdDevice>().Single(s => s.DeviceNo == trackingInfo.DestinationDeviceNo); 
                    dbClient.Updateable(transporDevice).AddQueue();
                    socketMsg.Status = SocketMessageStatus.Success.ToString();
                    socketMsg.Message = $"AGV申请放料箱通过";
                    _processWebSocketSend(socketMsg); 
                } 
            }
            //AGV任务已完成
            else if (args.method == AGVTaskStatus.End.ToString())
            { 
                var transporDevice = dbClient.Queryable<AutoProdDevice>().Single(s => s.DeviceNo == trackingInfo.DestinationDeviceNo);
                if (trackingInfo.IsReturn)
                { 
                    trackingInfo.TaskEndTime = DateTime.Now.ToString(); 
                    trackingInfo.IsExecuting = false;
                    transporDevice.IsUsing = false;
                    transporDevice.AGVTaskId = 0;  
                    socketMsg.Data.IsFinished = true;
                    socketMsg.Data.TaskType = trackingInfo.TaskType;
                    socketMsg.Data.OrderNo = trackingInfo.OrderNo;
                }
                else
                {
                    trackingInfo.IsReturn = true;
                    socketMsg.Data.IsDelived = true;
                }
                dbClient.Updateable(transporDevice).AddQueue(); 
                socketMsg.Status = SocketMessageStatus.Success.ToString();
                socketMsg.Message = "AGV本次任务已完成!";
                _processWebSocketSend(socketMsg); 
            }
            //AGV任务已取消
            else if (args.method == AGVTaskStatus.Cancel.ToString())
            { 
                trackingInfo.IsExecuting = false; 
                var transporDevice = dbClient.Queryable<AutoProdDevice>().Single(s => s.DeviceNo == trackingInfo.DestinationDeviceNo);
                transporDevice.IsUsing = false;
                transporDevice.AGVTaskId = 0;
                dbClient.Updateable(transporDevice).AddQueue(); 
                socketMsg.Status = SocketMessageStatus.Failed.ToString();
                socketMsg.Message = "AGV本次任务已被取消!";
                _processWebSocketSend(socketMsg);
            }
            dbClient.Updateable(trackingInfo).AddQueue();
            dbClient.SaveQueues();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _webSocketService.OnDataReceived -= _processWebSocketReceive;
                    //_rcsService.OnTaskResult -= _onAgvCallback;
                }
                _disposed = true;
            }
        }

        ~AutoTransportHandler()
        {
            Dispose(false);
        }
    }
}
