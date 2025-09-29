using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Intrinsics.X86;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Result;

namespace External.MinIOService
{ 
    public class MinIOHelper
    {
        private readonly IMinioClient _minioClient;

        private readonly string ? _deftBucket;

        public MinIOHelper(IMinioClient minioClient, IConfiguration configuration)
        { 
            _minioClient = minioClient;
            _deftBucket = configuration.GetSection("MinIO:BucketName").Value;
        }

        public async Task ChkAndSetBucket()
        {
            var beArgs = new BucketExistsArgs()
                     .WithBucket(_deftBucket);
            bool found = await _minioClient.BucketExistsAsync(beArgs);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(_deftBucket);
                await _minioClient.MakeBucketAsync(mbArgs);
            }
        }

        public async Task<string> Save(string fileName, Stream stream, string fileType)
        {
            using (stream)
            {
                var args = new PutObjectArgs()
                 .WithBucket(_deftBucket)
                 .WithObject(fileType + "/" + fileName)
                 .WithStreamData(stream)
                 .WithObjectSize(stream.Length)
                 .WithContentType("application/octet-stream");
                    await _minioClient.PutObjectAsync(args);
                    var url = $"{_minioClient.Config.Endpoint}/{_deftBucket}/{fileType}/{fileName}";
                    return await Task.FromResult(url);
            } 
        }

        public async Task<Stream> GetFileStream(string fileName, string fileType)
        { 
            var ms=new MemoryStream();
            var args = new GetObjectArgs()
                .WithBucket(_deftBucket)
                .WithObject(fileType + "/" + fileName)
                .WithCallbackStream(st => st.CopyTo(ms));
            await _minioClient.GetObjectAsync(args);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public async Task<bool> Remove(string remotePath)
        {
            var pathArray= remotePath.Split('/');
            var fileName= pathArray[pathArray.Length - 1];
            var fileType= pathArray[pathArray.Length - 2];
            var args = new RemoveObjectArgs()
             .WithBucket(_deftBucket)
             .WithObject(fileType + "/" + fileName);
            await _minioClient.RemoveObjectAsync(args);
            return true;
        }
         
    }
}
