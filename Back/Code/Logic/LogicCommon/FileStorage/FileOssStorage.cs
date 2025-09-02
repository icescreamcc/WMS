using External.Aliyun.OSS;
using Models.Model.Baseinfo;
using Models.Model.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicCommon.FileStorage
{
    public class FileOssStorage : IFileStorage
    {
        private readonly OssHelper _ossHelper;
        public FileOssStorage(OssHelper ossHelper)
        {
            _ossHelper = ossHelper;
        } 

        public async Task<Stream> GetFileStream(string filePath)
        {
            return await _ossHelper.GetFile(filePath);
        }

        public async Task<string> SaveFile(string fileName, Stream stream, FileType fileType)
        {
            return await _ossHelper.Save(fileName, stream, fileType.ToString());
        }

        public async Task<FileModel> SaveFileToModel(string fileName, Stream stream, FileType fileType)
        {
           var url= await _ossHelper.Save(fileName, stream, fileType.ToString());
            return new FileModel
            {
                FullPath = url,
                Path = ""
            };
        }

        public Task<FileModel> SaveImg(string fileName, Stream stream)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteFile(string path)
        {
           await _ossHelper.Remove(path);
        }

        public Task<FileInfoDto> GetFileAndSave(string fileName, Stream stream, FileType fileType)
        {
            throw new NotImplementedException();
        }
    }
}
