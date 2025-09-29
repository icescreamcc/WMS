using AutoMapper;
using DbRepository.Repository;
using DbRepository.Repository.DbModels;
using External.Common; 
using External.Log;
using Logic.LogicBase;
using Logic.LogicBase.CacheService;
using Microsoft.Extensions.Configuration;
using Models.Model.Sys; 
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System;
using System.Net;
using ExcelDataReader;
using NPOI.SS.Formula.Functions;
using System.Security.Policy;
using Aliyun.OSS;
using Renci.SshNet;
using Models.Model.AutomationDevice;


namespace Logic.Sys
{
    public class ImportIBDDataSync : ExternalApiHandler
    {  
        private readonly string _porvider;

        private readonly string _secret;

        private readonly string _host;

        private  string _accessToken;

        private readonly IMapper _mapper;

        private readonly LogHelper _logHelper;  

        public ImportIBDDataSync(Repository repository, IConfiguration configuration, IMapper mapper, LogHelper logHelper, BusinessCacheService businessCacheService, BusinessCacheItem businessCacheItem, HttpHelperAsync httpHelperAsync) : base(repository, businessCacheService, businessCacheItem, httpHelperAsync)
        {
            _mapper = mapper;
            _logHelper = logHelper;  
        }

        public  void Sync()
        {
            _logHelper.LogInfo("ImportIBDDataSync.Sync", "开始获取Excel数据", null);
            //获取今天的日期
            DateTime newDateTime=DateTime.Now;
            int newDate = Int32.Parse(newDateTime.Year.ToString() + newDateTime.Month.ToString("D2") + newDateTime.Day.ToString("D2"));//今天的日期

            string host = "10.24.78.122";
            int port = 22;
            string ftpPath = "/SCM/Packaging management system/SAP Data/"+ newDate + ".XLSX";
            //string ftpPath = "/SCM/Packaging management system/SAP Data/20250205.XLSX";
            string localPath = "local-file.XLSX";
            string ftpUser = "sftpuser";
            string ftpPassword = "Csconti0407";
           


            try
            {
                //DownloadFileFromFTP1(host, localPath, ftpUser, ftpPassword);
                // 从FTP下载文件
                 SFTPHelper.DownloadFileFromFTP(host, port, ftpPath, localPath, ftpUser, ftpPassword);
                //DownloadFileFromFTP(ftpPath, localPath, ftpUser, ftpPassword);

                // 读取Excel内容
                DataTable data = ReadExcel(localPath);

                var grourData = from row in data.AsEnumerable()
                                group row by new
                                {
                                    PurchasingDocument = row.Field<string>("Purchasing Document"),
                                    Material =row.Field<string>("Material"),
                                    Nameofvendor = row.Field<string>("Name of vendor")
                                } into grp
                                select new
                                {
                                    PurchasingDocument = grp.Key.PurchasingDocument,
                                    Material = grp.Key.Material,
                                    Nameofvendor = grp.Key.Nameofvendor,
                                    //TotalSalary = grp.Sum(r => r.Field<int>("Delivery quantity"))
                                };
                foreach (var item in grourData)
                {
                    var pu = item.PurchasingDocument;
                    var ma = item.Material;
                    var na = item.Nameofvendor;
                    //var sums = item.TotalSalary;
                    //查询data中对应的数据进行计算
                    int sumNum = 0;
                    var sumData = from row in data.AsEnumerable()
                                  where row.Field<string>("Purchasing Document") == pu &&
                                       row.Field<string>("Material") == ma &&
                                       row.Field<string>("Name of vendor") == na 
                                 select row;
                    string asnCheckStatus = "";
                    foreach (var itemSum in sumData)
                    {
                        if (asnCheckStatus=="")
                        {
                            asnCheckStatus = itemSum[8].ToString();
                        }
                        int quantity =Int32.Parse(itemSum[4].ToString());
                        sumNum += quantity;
                    }
                    //去数据库中查询对应的数据进行对比（只取DetailStatus未收货的状态的)
                    var ReceivingData =  Repository.ClientDb.Queryable<ReceivingOrderDetail>()
                        .Where(m=>m.GoodsNo==ma && m.ExternalOrderNo== pu && m.SupplierName==na )
                        .Where(m=>m.DetailStatus== "Pending"|| m.DetailStatus== "Approvaling" || m.DetailStatus== "WaitReceiving")
                        .ToList();

                    //与数据库进行比较。大于等于，显示修改列ASNCheckStatus 为data中 External Delivery ID这个列的数据.否则不添加
                    if (ReceivingData.Count > 0)
                    {
                        int receivingNum = 0;
                        for (int i = 0; i < ReceivingData.Count; i++)
                        {
                            int dataNum = Int32.Parse(ReceivingData[i].Quantity.ToString());
                            receivingNum += dataNum;
                        }
                        if (sumNum >= receivingNum)
                        {
                            //修改数据库
                            for (int i = 0; i < ReceivingData.Count; i++)
                            {
                                var model = _mapper.Map<ReceivingOrderDetail>(ReceivingData[i]);
                                model.ASNCheckStatus = "Y";
                                //model.ASNCheckStatus = asnCheckStatus;
                                Repository.ClientDb.Updateable(model).UpdateColumns(it => new { it.ASNCheckStatus }).ExecuteCommand();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ReceivingData.Count; i++)
                            {
                                var model = _mapper.Map<ReceivingOrderDetail>(ReceivingData[i]);
                                model.ASNCheckStatus = "N";
                                //Repository.ClientDb.Updateable(model).AddQueue();
                                Repository.ClientDb.Updateable(model).UpdateColumns(it => new { it.ASNCheckStatus }).ExecuteCommand();
                            }

                           
                        }
                    }
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }

          
        }
        public static void DownloadFileFromFTP(string ftpPath, string localPath, string user, string passworda)
        {
            string host = "10.24.78.122";
            int port = 22;
            string username = user;
            string password = passworda;
            string remoteFilePath = ftpPath;
            string localFilePath = localPath;

            // 创建 SFTP 客户端对象
            using (var sftp = new SftpClient(host, port, username, password))
            {
                try
                {
                    // 连接到 SFTP 服务器s
                    sftp.Connect();

                    // 检查文件是否存在
                    if (sftp.Exists(remoteFilePath))
                    {
                        Console.WriteLine($"文件 '{remoteFilePath}' 存在，正在读取...");

                        // 下载文件到本地
                        using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                        {
                            sftp.DownloadFile(remoteFilePath, fileStream);
                            Console.WriteLine($"文件已下载到 {localFilePath}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"文件 '{remoteFilePath}' 不存在！");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"连接或读取文件时出错: {ex.Message}");
                }
                finally
                {
                    // 断开连接
                    sftp.Disconnect();
                }
            }
        }
        public static void DownloadFileFromFTP1(string ftpPath, string localPath, string user, string password)
        {
            

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(user, password);
            request.KeepAlive = false;
            request.UseBinary = true;
            request.UsePassive = false;
            WebResponse responses = request.GetResponse();
            StreamReader reader = new StreamReader(responses.GetResponseStream());//中文文件名

            // 使用WebResponse获取响应
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                // 获取文件流
                Stream responseStream = response.GetResponseStream();

                // 创建文件流写入器，将数据写入本地文件
                using (FileStream fileStream = new FileStream(localPath, FileMode.Create))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
          

        }

        public static DataTable ReadExcel(string filePath)
        {
            // 解决中文编码问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // 配置读取选项（适配.xls和.xlsx）
                var configuration = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true // 是否使用第一行为列头
                    }
                };

                // 读取为DataSet（支持多Sheet）
                DataSet result = reader.AsDataSet(configuration);
                return result.Tables[0]; // 返回第一个Sheet的数据
            }
        }
    }
}
