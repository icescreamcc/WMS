using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
namespace External.Common
{
   public class SFTPHelper
    {
        public static void DownloadFileFromFTP(string host,int port, string ftpPath, string localPath, string user, string password)
        {
            //string host = "10.24.78.122";
            //string username = "sftpuser";
            //string password = "Csconti0407";
            //string ftpPath = "/SCM/Packaging management system/SAP Data/20250205.XLSX";
            //string localPath = "local-file.XLSX";
            using (var sftp = new SftpClient(host,port, user, password))
            {
                try
                {
                    // 连接到 SFTP 服务器
                    sftp.Connect();

                    // 检查文件是否存在
                    if (sftp.Exists(ftpPath))
                    {
                        Console.WriteLine($"文件 '{ftpPath}' 存在，正在读取...");

                        // 下载文件到本地
                        using (var fileStream = new FileStream(localPath, FileMode.Create))
                        {
                            sftp.DownloadFile(ftpPath, fileStream);
                            Console.WriteLine($"文件已下载到 {localPath}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"文件 '{localPath}' 不存在！");
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

    } 
}
