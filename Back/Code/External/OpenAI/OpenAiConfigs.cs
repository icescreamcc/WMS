using External.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI
{
   public class OpenAiConfigs
    {
        public string User { get; set; }

        public string ApiKey { get; set; }

        public string OrgId { get; set; }

        public string ModelsUrl { get; set; }

        public string ChatUrl { get; set; }

        public string CompletionUrl { get; set; }

        public string ImageCreateUrl { get; set; }

        public string ImageEditUrl { get; set; }

        public string ImageVariationUrl { get; set; }

        public string AudioTranscriptionsUrl { get; set; }

        public string AudioTranslationsUrl { get; set; }

        public string CompletionModelId { get; set; }

        public string ChatModelId { get; set; }

        public string AudioModelId { get; set; }
         

        public OpenAiConfigs(IConfiguration configuration)
        {
            User = configuration.GetSection("OpenAi:User").Value;
            ApiKey = EncryptionHelper.DesDecrypt(configuration.GetSection("OpenAi:ApiKey").Value);
            OrgId = EncryptionHelper.DesDecrypt(configuration.GetSection("OpenAi:Organization").Value);
            ModelsUrl = configuration.GetSection("OpenAi:ModelsUrl").Value;
            CompletionUrl = configuration.GetSection("OpenAi:CompletionUrl").Value;
            ChatUrl = configuration.GetSection("OpenAi:ChatUrl").Value;
            ImageCreateUrl = configuration.GetSection("OpenAi:ImageCreateUrl").Value;
            ImageEditUrl = configuration.GetSection("OpenAi:ImageEditUrl").Value;
            ImageVariationUrl = configuration.GetSection("OpenAi:ImageVariationUrl").Value;
            AudioTranscriptionsUrl = configuration.GetSection("OpenAi:AudioTranscriptionsUrl").Value;
            AudioTranslationsUrl = configuration.GetSection("OpenAi:AudioTranslationsUrl").Value;
            CompletionModelId = configuration.GetSection("OpenAi:CompletionModelId").Value;
            ChatModelId = configuration.GetSection("OpenAi:ChatModelId").Value;
            AudioModelId = configuration.GetSection("OpenAi:AudioModelId").Value;

        }
    }
}
