using External.Socket.TCP;
using Newtonsoft.Json.Linq;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace External.Socket.PLC
{
    public class SiemensS7Client : IDisposable
    {
        public string? DeviceNo { get; set; }

        private Plc? _plc;

        public SiemensS7Client()
        {
        }

        public SiemensS7Client(string deviceNo)
        {
            DeviceNo = deviceNo;
        }

        /// <summary>
        /// 建立连接事件处理
        /// </summary>
        public event EventHandler<PLCEventArgs>? OnClientConnected;

        /// <summary>
        /// 连接失败事件处理
        /// </summary>
        public event EventHandler<PLCEventArgs>? OnClientConnectFailed;

        /// <summary>
        /// 接收到数据事件处理
        /// </summary>
        public event EventHandler<PLCEventArgs>? OnDataReceived;

        /// <summary>
        /// 连接断开事件处理
        /// </summary>
        public event EventHandler<PLCEventArgs>? OnClientDisconnected;

        /// <summary>
        /// 接收数据失败事件处理
        /// </summary>
        public event EventHandler<PLCEventArgs>? OnDataReceivedFailed;

        /// <summary>
        /// 发送数据成功事件处理
        /// </summary>
        public event EventHandler<PLCEventArgs>? OnSendSuccess;

        /// <summary>
        /// 发送数据失败事件处理
        /// </summary>
        public event EventHandler<PLCEventArgs>? OnSendFailed;


        public bool IsConnected
        {
            get
            {
                if (_plc == null)
                {
                    return false;
                }
                else
                {
                    return _plc.IsConnected;
                }
            }
        }

        public async Task ConnectAsync(string ip, CpuType cupType, short rack = 0, short solt = 1)
        {
            if (!IsConnected)
            {
                if (_plc == null)
                {
                    _plc = new Plc(cupType, ip, rack, solt);
                }
                try
                {
                    _plc.ReadTimeout = 2000;
                    _plc.WriteTimeout = 2000;
                    await _plc.OpenAsync();
                    if (OnClientConnected != null)
                    {
                        OnClientConnected(this, new PLCEventArgs(_plc, "连接成功！"));
                    }
                }
                catch (Exception ex)
                {
                    if (OnClientConnectFailed != null)
                    {
                        OnClientConnectFailed(this, new PLCEventArgs(_plc, "连接失败，" + ex.Message));
                    }
                }
            }
        }

        public void ReadDurable(string dbAddress, int binRank=0, int interval = 2000)
        {
            var timer = new Timer();
            timer.Interval = interval;
            timer.Elapsed += (sender, e) => ReadEvent(sender, e, dbAddress, binRank, timer);
            timer.Start();
        }

        public void ReadEvent(object? sender, ElapsedEventArgs e, string db, int binRank, Timer timer)
        {
            if (_plc != null)
            {
                if (_plc.IsConnected)
                {
                    try
                    {
                        var val = _plc.Read(db);
                        if (OnDataReceived != null)
                        {
                            OnDataReceived(this, new PLCEventArgs(_plc, val, db, binRank));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (OnDataReceivedFailed != null)
                        {
                            OnDataReceivedFailed(this, new PLCEventArgs(_plc, db, $"读取数据失败，DB：{db}，错误：{ex.Message}"));
                        }
                    }
                }
                else
                {
                    if (OnClientDisconnected != null)
                    {
                        OnClientDisconnected(this, new PLCEventArgs(_plc, "读取数据失败，未和PLC建立连接或已断开连接"));
                    }
                    if (timer != null)
                    {
                        timer.Stop();
                    }
                }
            }
            else
            {
                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected(this, new PLCEventArgs("读取数据失败，PLC客户端引用为空"));
                }
                if (timer != null)
                {
                    timer.Stop();
                }
            }
        }

        public void WriteDurable<T>(string dbAddress, object value, int binRank=0, int interval = 2000)
        {
            var timer = new Timer();
            timer.Interval = interval;
            timer.Elapsed += (sender, e) => WriteEvent<T>(sender, e, dbAddress, value, binRank, timer);
            timer.Start();
        }

        public void WriteEvent<T>(object? sender, ElapsedEventArgs e, string db, object value, int binRank, Timer timer)
        {
            if (_plc != null)
            {
                if (_plc.IsConnected)
                {
                    try
                    {
                        T? v = _connvertPLCValue<T>(value);
                        _plc.Write(db, v);
                        if (OnSendSuccess != null)
                        {
                            OnSendSuccess(this, new PLCEventArgs(_plc, value, db, binRank));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (OnSendFailed != null)
                        {
                            OnSendFailed(this, new PLCEventArgs(_plc, value, db, $"写入数据失败，DB:{db}，Value:{value}，错误：{ex.Message}"));
                        }
                    }
                }
                else
                {
                    if (OnClientDisconnected != null)
                    {
                        OnClientDisconnected(this, new PLCEventArgs(_plc, "写入数据失败，未和PLC建立连接或已断开连接"));
                    }
                    if (timer != null)
                    {
                        timer.Stop();
                    }
                }
            }
            else
            {
                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected(this, new PLCEventArgs("写入数据失败，PLC客户端引用为空"));
                }
                if (timer != null)
                {
                    timer.Stop();
                }
            }
        }

        public T? Read<T>(string dbAddress, int binRank = 0)
        {
            if (_plc != null)
            {
                try
                {
                    if (_plc.IsConnected)
                    {
                        var val = _plc.Read(dbAddress);
                        if (OnDataReceived != null)
                        {
                            OnDataReceived(this, new PLCEventArgs(_plc, val, dbAddress, binRank));
                        }
                        return _connvertPLCValue<T>(val);
                    }
                    else
                    {
                        if (OnClientDisconnected != null)
                        {
                            OnClientDisconnected(this, new PLCEventArgs(_plc, "读取数据失败，未和PLC建立连接或已断开连接"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (OnDataReceivedFailed != null)
                    {
                        OnDataReceivedFailed(this, new PLCEventArgs(_plc, dbAddress, $"读取数据失败，DB：{dbAddress}，错误：{ex.Message}"));
                    }
                }
            }
            else
            {
                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected(this, new PLCEventArgs("读取数据失败，PLC客户端引用为空"));
                }
            }
            return default;
        }

        public async Task<T?> ReadAsync<T>(string dbAddress, int binRank = 0)
        {
            if (_plc != null)
            {
                try
                {
                    if (_plc.IsConnected)
                    {
                        var val = await _plc.ReadAsync(dbAddress);
                        if (OnDataReceived != null)
                        {
                            OnDataReceived(this, new PLCEventArgs(_plc, val, dbAddress, binRank));
                        }
                        return _connvertPLCValue<T>(val);
                    }
                    else
                    {
                        if (OnClientDisconnected != null)
                        {
                            OnClientDisconnected(this, new PLCEventArgs(_plc, "读取数据失败，未和PLC建立连接或已断开连接"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (OnDataReceivedFailed != null)
                    {
                        OnDataReceivedFailed(this, new PLCEventArgs(_plc, dbAddress, $"读取数据失败，DB：{dbAddress}，错误：{ex.Message}"));
                    }
                }
            }
            else
            {
                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected(this, new PLCEventArgs("读取数据失败，PLC客户端引用为空"));
                }
            }
            return default;
        }

        public void Write<T>(string dbAddress, object val, int binRank = 0)
        {
            if (_plc != null)
            {
                if (_plc.IsConnected)
                {
                    try
                    {
                        T? v = _connvertPLCValue<T>(val);
                        _plc.Write(dbAddress, v);
                        if (OnSendSuccess != null)
                        {
                            OnSendSuccess(this, new PLCEventArgs(_plc, val, dbAddress, binRank));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (OnSendFailed != null)
                        {
                            OnSendFailed(this, new PLCEventArgs(_plc, val, dbAddress, $"写入数据失败，DB:{dbAddress}，Value:{val}，错误：{ex.Message}"));
                        }
                    }
                }
                else
                {
                    if (OnClientDisconnected != null)
                    {
                        OnClientDisconnected(this, new PLCEventArgs(_plc, "写入数据失败，未和PLC建立连接或已断开连接"));
                    }
                }
            }
            else
            {
                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected(this, new PLCEventArgs("写入数据失败，PLC客户端引用为空"));
                }
            }
        }

        public async Task WriteAsync<T>(string dbAddress, object val, int binRank = 0)
        {
            if (_plc != null)
            {
                if (_plc.IsConnected)
                {
                    try
                    {
                        T? v = _connvertPLCValue<T>(val);
                        await _plc.WriteAsync(dbAddress, v);
                        if (OnSendSuccess != null)
                        {
                            OnSendSuccess(this, new PLCEventArgs(_plc, val, dbAddress, binRank));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (OnSendFailed != null)
                        {
                            OnSendFailed(this, new PLCEventArgs(_plc, val, dbAddress, $"写入数据失败，DB:{dbAddress}，Value:{val}，错误：{ex.Message}"));
                        }
                    }
                }
                else
                {
                    if (OnClientDisconnected != null)
                    {
                        OnClientDisconnected(this, new PLCEventArgs(_plc, "写入数据失败，未和PLC建立连接或已断开连接"));
                    }
                }
            }
            else
            {
                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected(this, new PLCEventArgs("写入数据失败，PLC客户端引用为空"));
                }
            }
        }

        public void Close()
        {
            if (_plc != null)
            {
                _plc.Close();
                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected(this, new PLCEventArgs(_plc, "与PLC连接已主动断开"));
                }
            }
        }

        private T? _connvertPLCValue<T>(object? val)
        {
            var valStr = val?.ToString();
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(valStr, out bool res))
                {
                    return (T)(object)res;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (int.TryParse(valStr, out int res))
                {
                    return (T)(object)res;
                }
            }
            else if (typeof(T) == typeof(short))
            {
                if (short.TryParse(valStr, out short res))
                {
                    return (T)(object)res;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (float.TryParse(valStr, out float res))
                {
                    return (T)(object)res;
                }
            }
            else if (typeof(T) == typeof(double))
            {
                if (double.TryParse(valStr, out double res))
                {
                    return (T)(object)res;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)valStr;
            }
            else if (typeof(T) == typeof(char))
            {
                if (char.TryParse(valStr, out char res))
                {
                    return (T)(object)res;
                }
            }
            return default;
        }

        public void Dispose()
        {

        }
    }
}
