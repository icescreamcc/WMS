using External.Common;
using External.MinIOService;
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
    public class FileMinIoStorage : IFileStorage
    {
        private readonly MinIOHelper  _minIOHelper;

        private readonly FileHelper _fileHelper;

        public FileMinIoStorage(MinIOHelper minIOHelper, FileHelper fileHelper)
        {
            _minIOHelper = minIOHelper;
            _fileHelper = fileHelper;
        }

        public async Task DeleteFile(string path)
        {
            await _minIOHelper.Remove(path);
        }

        public async Task<FileInfoDto> GetFileAndSave(string fileName, Stream stream, FileType fileType)
        {
            var date = DateTime.Now.ToString("yyyyMM");
            var savePath = @$"{Directory.GetCurrentDirectory()}\Files\{fileType}\{date}";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            savePath = $@"{savePath}\{fileName}";
            await _fileHelper.SaveFile(savePath, stream);
            using (var fileStream = File.OpenRead(savePath))
            {
                var fileurl = await _minIOHelper.Save(fileName, fileStream, fileType.ToString());
                return new FileInfoDto
                {
                    FileName = fileName,
                    Url = fileurl,
                    Path = savePath,
                    FileInfoType = fileType.ToString()
                };
            }
        }

        public async Task<Stream> GetFileStream(string filePath)
        {
            var pathArr= filePath.Split('/');
            var fileName= pathArr[pathArr.Length-1];
            var fileType = pathArr[pathArr.Length-2];
            return await _minIOHelper.GetFileStream(fileName, fileType);
        }

        public async Task<string> SaveFile(string fileName, Stream stream, FileType fileType)
        {
          return await _minIOHelper.Save(fileName, stream, fileType.ToString());
        }
         
    }
}
