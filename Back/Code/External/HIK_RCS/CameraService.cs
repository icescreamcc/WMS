using External.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS
{
    public class CameraService
    {
        private readonly HttpHelperAsync _httpHelperAsync;
            
        public CameraService(IConfiguration configuration, HttpHelperAsync httpHelperAsync)
        {
            _httpHelperAsync = httpHelperAsync; 
        }

        public T CaptureImage<T>(string ip,int port) where T : class
        {
            string url = $"http://{ip}:{port}/ISAPI/Streaming/channels/101/picture?snapShotImageType=JPEG";
            return _httpHelperAsync.RequestGet<T>(url);
        }
    }
}
