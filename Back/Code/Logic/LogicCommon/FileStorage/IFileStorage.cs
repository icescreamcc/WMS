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
    public interface IFileStorage
    { 
        public Task<Stream> GetFileStream(string filePath);

        public Task<string> SaveFile(string fileName, Stream stream, FileType fileType);

        public Task<FileInfoDto> GetFileAndSave(string fileName, Stream stream, FileType fileType);

        public Task DeleteFile(string path);
    }
}
