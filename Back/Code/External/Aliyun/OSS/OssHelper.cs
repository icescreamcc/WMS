using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Aliyun.OSS
{
   public class OssHelper
    {
        private readonly OssConfigs _configEnvironment;

        public OssHelper(OssConfigs configEnvironment)
        {
            _configEnvironment = configEnvironment;
        }

        /// <summary>
        /// 连接OSS
        /// </summary>
        /// <returns></returns>
        private OssClient _getOssClient()
        {
            return new OssClient(
                new Uri(_configEnvironment.Config.EndPoint),
                _configEnvironment.Config.KeyID,
                _configEnvironment.Config.KeySecret,
                new ClientConfiguration
                {
                    ConnectionTimeout = 300000,//设置连接超时时间
                    MaxErrorRetry = 3//设置请求发生错误时最大的重试次数
                });
        }

        #region Save

        /// <summary>
        /// 上传文件，如果文件存在，直接覆盖更新
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param> 
        public async Task<string> Save(string fileName, Stream stream,string fileType)
        {
            string bucketName = _configEnvironment.Config.BucketName;
            string remotePath = $"{_configEnvironment.Config.RootFolder}/{fileType}/{DateTime.Now.ToString("u")}/{Guid.NewGuid().ToString("N")}/{fileName}"; 
            string visitUrl = _configEnvironment.Config.EndPoint.Replace("http://", $"http://{bucketName}.") + "/" + remotePath;
            if (_configEnvironment.Config.BucketUrl != "")
            {
                visitUrl = _configEnvironment.Config.BucketUrl + "/" + remotePath;
            }
            ObjectMetadata metadata = new ObjectMetadata();
            metadata.ContentEncoding = Encoding.UTF8.ToString();
            PutObjectResult result = _getOssClient().PutObject(_configEnvironment.Config.BucketName, remotePath, stream, metadata);
            await Task.CompletedTask;
            return visitUrl;
        }

        /// <summary>
        /// 上传文件，如果文件存在，直接覆盖更新
        /// </summary>
        /// <param name="remoteFolderName">待保存在存储桶的相对文件夹</param>
        /// <param name="remoteFileName">待保存在存储桶的文件名</param>
        /// <param name="localFilePath">本地待上传的文件完整路径</param>
        /// <returns>上传文件的大小</returns>
        public async Task<long> Save(string remoteFolderName, string remoteFileName, string localFilePath)
        {
            string key = remoteFolderName.TrimEnd('/') + "/" + remoteFileName;
            PutObjectResult result = _getOssClient().PutObject(_configEnvironment.Config.BucketName, key, localFilePath);
            return await GetFileSize(remoteFolderName, remoteFileName);
        }
          
        
        #endregion

        #region Remove

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="remoteFolderName">文件在存储桶的相对文件夹</param>
        /// <param name="remoteFileName">文件在存储桶的文件名</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> Remove(string remoteFolderName, string remoteFileName)
        {
            string key = remoteFolderName.TrimEnd('/') + "/" + remoteFileName;
            DeleteObjectResult result = _getOssClient().DeleteObject(_configEnvironment.Config.BucketName, key);
            return await Task.FromResult(result.DeleteMarker);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="remotePath">Bucket中的存放路径</param>
        /// <returns></returns>
        public async Task<bool> Remove(string remotePath)
        {
            DeleteObjectResult result = _getOssClient().DeleteObject(_configEnvironment.Config.BucketName, remotePath);
            return await Task.FromResult(result.DeleteMarker);
        }

        #endregion

        #region GetFile

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="remotePath">文件在存储桶的相对文件夹和完整文件名称</param>
        /// <returns></returns>
        public async Task<Stream> GetFile(string remotePath)
        {
            OssObject result = _getOssClient().GetObject(_configEnvironment.Config.BucketName, remotePath);
            using (MemoryStream stream = new MemoryStream())
            {
                result.Content.CopyTo(stream);
                stream.Position = 0;
                return await Task.FromResult(stream);
            }
         
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="remoteFolderName">文件在存储桶的相对文件夹</param>
        /// <param name="remoteFileName">文件在存储桶的文件名</param>
        /// <returns>文件Stream</returns>
        public async Task<Stream> GetFile(string remoteFolderName, string remoteFileName)
        {
            string key = remoteFolderName.TrimEnd('/') + "/" + remoteFileName;
            OssObject result = _getOssClient().GetObject(_configEnvironment.Config.BucketName, key);
            return await Task.FromResult(result.Content);
        }

        /// <summary>
        /// 获取文件并保存
        /// </summary>
        /// <param name="remoteFolderName">文件在存储桶的相对文件夹</param>
        /// <param name="remoteFileName">文件在存储桶的文件名</param>
        /// <param name="fileToDownload">本地存储下载文件的目录</param>
        /// <returns>文件的字节数</returns>
        public async Task GetFile(string remoteFolderName, string remoteFileName, string fileToDownload)
        {
            string key = remoteFolderName.TrimEnd('/') + "/" + remoteFileName;
            OssObject result = _getOssClient().GetObject(_configEnvironment.Config.BucketName, key);
            //将从OSS读取到的文件写到本地
            using (var requestStream = result.Content)
            {
                byte[] buf = new byte[1024];
                FileStream fs = File.Open(fileToDownload, FileMode.OpenOrCreate);
                var len = 0;
                while ((len = requestStream.Read(buf, 0, 1024)) != 0)
                {
                    fs.Write(buf, 0, len);
                }
                await Task.CompletedTask;
            }
        }

        #endregion

        #region 其他

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="remoteFolderName">文件在存储桶的相对文件夹</param>
        /// <param name="remoteFileName">文件在存储桶的文件名</param>
        /// <returns>文件的字节数</returns>
        public async Task<long> GetFileSize(string remoteFolderName, string remoteFileName)
        {
            string key = remoteFolderName.TrimEnd('/') + "/" + remoteFileName;
            OssObject result = _getOssClient().GetObject(_configEnvironment.Config.BucketName, key);
            return await Task.FromResult(result.Metadata.ContentLength);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="remoteFolderName">文件在存储桶的相对文件夹</param>
        /// <param name="remoteFileName">文件在存储桶的文件名</param>
        /// <returns>是否存在</returns>
        public async Task<bool> IsExist(string remoteFolderName, string remoteFileName)
        {
            return await GetFileSize(remoteFolderName, remoteFileName) > 0;
        }

        /// <summary>
        /// 获取文件访问路径
        /// </summary>
        /// <param name="remoteFolderName">文件在存储桶的相对文件夹</param>
        /// <param name="remoteFileName">文件在存储桶的文件名</param>
        /// <returns></returns>
        public string GetFileUrl(string remoteFolderName, string remoteFileName)
        {
            return _configEnvironment.Config.EndPoint.Replace("http://", "http://" + _configEnvironment.Config.BucketName + ".")
                + "/" + remoteFolderName.TrimEnd('/')
                + "/" + remoteFileName;
        }

        #endregion
    }
}
