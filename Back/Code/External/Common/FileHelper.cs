using Microsoft.Extensions.Configuration;
using Models.Model.Enum;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace External.Common
{
   public class FileHelper
    {
        private readonly IConfiguration _configuration;

        public FileHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 将二进制写入文件
        /// </summary>
        /// <param name="path">要保存的文件路径</param>
        /// <param name="buffer">文件二进制</param>
        public  void SaveFile(string path, byte[] buffer)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream fileStream = new FileStream(path, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            binaryWriter.Write(buffer);
            binaryWriter.Close();
        }

        /// <summary>
        /// 将String写入文件
        /// </summary>
        /// <param name="path">要保存的文件路径</param>
        /// <param name="fileContent">文件String值</param>
        public  void SaveFile(string path, string fileContent)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine(fileContent);
            streamWriter.Close();
        }


        /// <summary>
        /// 将Stream写入文件
        /// </summary>
        /// <param name="path">要保存的文件路径</param>
        /// <param name="stream">文件的stream</param>
        public async Task SaveFile(string path, Stream stream)
        {
            using (var fs = File.Create(path))
            {
                using (stream)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    await stream.CopyToAsync(fs);
                } 
            }
        }

        /// <summary>
        /// 读取文件二进制
        /// </summary>
        /// <param name="path">要读取的文件路径</param>
        /// <returns></returns>
        public  byte[] ReadFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }
         

        /// <summary>
        /// 读取文件到Stream
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public  MemoryStream ReadFileStream(string path)
        {
            if (File.Exists(path))
            {
                return new MemoryStream(File.ReadAllBytes(path));
            }
            return null;
        }

        /// <summary>
        /// 读取文件文本
        /// </summary>
        /// <param name="path">要读取的文件路径</param>
        /// <returns></returns>
        public  string ReadFileString(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "";
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public  async Task DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 遍历文件夹中的所有文件
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="dic"></param>
        public  void GetDirectoryFiles(IList<FileInfo> fileList, DirectoryInfo dic)
        {
            foreach (FileInfo file in dic.GetFiles())
            {
                fileList.Add(file);
            }
            foreach (DirectoryInfo subdic in dic.GetDirectories())
            {
                GetDirectoryFiles(fileList, subdic);
            }
        }

        public  string ConvertImageToBase64(string imagePath)
        { 
            byte[] imageBytes = File.ReadAllBytes(imagePath); 
            string base64Image = Convert.ToBase64String(imageBytes);
            return base64Image;
        }

        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="imageUrl"></param> 
        /// <param name="quality"></param>
        /// <returns></returns>
        public async Task<string> CompressImageFromUrlAsync(string imageUrl, int quality)
        {
            var savePath = @$"{Directory.GetCurrentDirectory()}\Files\Thumbnail"; 
            var imageUrlSplit = imageUrl.Split('/');
            var fileName = imageUrlSplit[imageUrlSplit.Length - 1];
            var host = _configuration.GetSection("FileStorage:Host").Value;
            var saveFullPath = @$"{Directory.GetCurrentDirectory()}\Files\Thumbnail\{fileName}";
            var outputFilePath = savePath + @"\" + fileName;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            if(!File.Exists(saveFullPath))
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] imageData = await client.GetByteArrayAsync(imageUrl);
                    using (Image image = Image.Load(imageData))
                    {
                        var encoder = new JpegEncoder
                        {
                            Quality = quality
                        };
                        image.Save(outputFilePath, encoder);
                    }
                }
            } 
            return @$"{host}/static/Thumbnail/{fileName}";
        }
    }
}
