using External.Common;
using Microsoft.Extensions.Configuration;
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
    public class FileLocalStorage : IFileStorage
    {
        private readonly IConfiguration _configuration;

        private readonly FileHelper _fileHelper;

        public FileLocalStorage(IConfiguration configuration, FileHelper fileHelper)
        {
            _configuration = configuration;
            _fileHelper = fileHelper;
        }

        public async Task<Stream> GetFileStream(string filePath)
        {
           var stream= _fileHelper.ReadFileStream(filePath);
            return await Task.FromResult(stream);
        }

        public async Task<string> SaveFile(string fileName, Stream stream, FileType fileType)
        {
            var date = DateTime.Now.ToString("yyyyMM");
            var savePath = @$"{Directory.GetCurrentDirectory()}\Files\{fileType}\{date}";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            savePath = $@"{savePath}\{fileName}";
            await _fileHelper.SaveFile(savePath, stream);
            var host = _configuration.GetSection("FileStorage:Host").Value; 
            return await Task.FromResult(@$"{host}/static/{fileType}/{date}/{fileName}");
        }

        public async Task<FileModel> SaveFileToModel( string fileName, Stream stream, FileType fileType)
        {
            var date = DateTime.Now.ToString("yyyyMM");
            var savePath = @$"{Directory.GetCurrentDirectory()}\Files\{fileType}\{date}";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            savePath = $@"{savePath}\{fileName}";
            await _fileHelper.SaveFile(savePath, stream);
            var host = _configuration.GetSection("FileStorage:Host").Value;
            var res = new FileModel
            {
                FullPath = @$"{host}/static/{fileType}/{date}/{fileName}",
                Path = @$"\Files\{fileType}\{date}\{fileName}"
            };
            return await Task.FromResult(res);
        }

        public async Task<FileModel> SaveImg( string fileName, Stream stream)
        {
            FileType fileType = FileType.Image;
            var date = DateTime.Now.ToString("yyyyMM");
            var savePath = @$"{Directory.GetCurrentDirectory()}\Files\{fileType}\{date}";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath); 
            }
            savePath = $@"{savePath}\{fileName}";
            await _fileHelper.SaveFile(savePath, stream);
            var host = _configuration.GetSection("FileStorage:Host").Value;
            var res = new FileModel
            {
                FullPath = $"{host}/static/{fileType}/{date}/{fileName}",
                Path = @$"\Files\{fileType}\{date}\{fileName}"
            };
            return await Task.FromResult(res);
        }

        public async Task DeleteFile(string path)
        { 
            var fullPath = $"{Directory.GetCurrentDirectory()}{path}"; 
            await _fileHelper.DeleteFile(fullPath);
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
            var host = _configuration.GetSection("FileStorage:Host").Value;
            var fileurl = @$"{host}/static/{fileType}/{date}/{fileName}";
            return new FileInfoDto
            {
                FileName= fileName,
                Url=fileurl,
                Path= savePath,
                FileInfoType=fileType.ToString()
            };

        }
    }
}
