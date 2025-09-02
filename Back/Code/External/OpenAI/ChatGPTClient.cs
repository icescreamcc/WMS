using External.OpenAI.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace External.OpenAI
{
    public class ChatGPTClient
    {
        private readonly OpenAiConfigs _openAiConfigs;

        private readonly HttpClient _httpClient;

        public ChatGPTClient(OpenAiConfigs openAiConfigs, HttpClient httpClient)
        {
            _openAiConfigs = openAiConfigs;
            _httpClient = httpClient;
            _httpClient.Timeout= new TimeSpan(0, 0, 20);
        }

        public double _deftTemperature { get => 1; }

        public int _deftMax_tokens { get => 2000; }

        public double _deftTop_p { get => 1; }

        public async Task<ModelList> GetModels()
        {

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAiConfigs.ApiKey}");
            var response = await _httpClient.GetAsync(_openAiConfigs.ModelsUrl);
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
                return JsonSerializer.Deserialize<ModelList>(content);
            else
                return default;
        }

        public async Task<ModelData> GetModelDetail(string modelId)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAiConfigs.ApiKey}");
            var response = await _httpClient.GetAsync($"{_openAiConfigs.ModelsUrl}/{modelId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
                return JsonSerializer.Deserialize<ModelData>(content);
            else
                return default;
        }

        /// <summary>
        /// 给定一个提示，模型将返回一个或多个预测的补全结果，并且还可以返回每个位置上替代标记的概率
        /// </summary>
        /// <param name="text"></param>
        /// <param name="temperature"></param>
        /// <param name="maxToken"></param>
        /// <param name="topP"></param>
        /// <returns></returns>
        public async Task<CompletionResponseDto> Completion(string text,double temperature,int maxToken,int topP)
        {
            var requestBody = new CompletionRequestDto
            {
                model = _openAiConfigs.CompletionModelId,
                prompt = text,
                max_tokens = maxToken == 0 ? _deftMax_tokens : maxToken,
                temperature = temperature == 0 ? _deftTemperature : temperature,
                top_p = topP == 0 ? _deftTop_p : topP,
                user=_openAiConfigs.User
            };
            return await _postApi<CompletionRequestDto, CompletionResponseDto>(requestBody, _openAiConfigs.CompletionUrl); 
        }

        /// <summary>
        /// 给定描述对话的消息列表，模型将返回一个响应。
        /// </summary>
        /// <param name="role"></param>
        /// <param name="text"></param>
        /// <param name="temperature"></param>
        /// <param name="maxToken"></param>
        /// <param name="topP"></param>
        /// <returns></returns>
        public async Task<ChatResponseDto> Chat(ChatRole role, string text, double temperature, int maxToken, int topP)
        {
            var requestBody = new ChatRequestDto
            {
                model = _openAiConfigs.ChatModelId,
                messages=new List<MessagesDto> { new MessagesDto { role = role.ToString(), content = text } },
                max_tokens = maxToken == 0 ? _deftMax_tokens : maxToken,
                temperature = temperature == 0 ? _deftTemperature : temperature,
                top_p = topP == 0 ? _deftTop_p : topP,
                user = _openAiConfigs.User
            };
            return await _postApi<ChatRequestDto, ChatResponseDto>(requestBody, _openAiConfigs.ChatUrl); 
        }

        /// <summary>
        /// 根据提示创建一个图片
        /// </summary>
        /// <param name="text"></param>
        /// <param name="n"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<ImageResponseDto> ImagesCreate(ImageRequestDto input)
        {
            input.user = _openAiConfigs.User;
            return await _postApi<ImageRequestDto, ImageResponseDto>(input, _openAiConfigs.ImageCreateUrl);
        }

        /// <summary>
        /// 根据原始图像和提示创建编辑或扩展的图像。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ImageResponseDto> ImagesEdit(ImageEditRequestDto input)
        {
            input.user = _openAiConfigs.User;
            return await _postApi<ImageRequestDto, ImageResponseDto>(input, _openAiConfigs.ImageEditUrl);
        }

        /// <summary>
        /// 根据原始图像和提示创建编辑或扩展的图像。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ImageResponseDto> ImagesVariation(ImageVariationRequestDto input)
        {
            input.user = _openAiConfigs.User;
            return await _postApi<ImageRequestDto, ImageResponseDto>(input, _openAiConfigs.ImageEditUrl);
        }

        /// <summary>
        /// 将音频转录成输入语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AudioResponseDto> AudioToText(byte[] file,  string prompt, double temperature,string language)
        {
            var input = new AudioRequestDto
            {
                file=file,
                model=_openAiConfigs.AudioModelId,
                prompt=prompt,
                response_format="text",
                temperature=temperature,
                language= language
            };
            return await _postApi<AudioRequestDto, AudioResponseDto>(input, _openAiConfigs.AudioTranscriptionsUrl);
        }
         

        private async Task<TResponse> _postApi<TRequest, TResponse>(TRequest input,string url)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAiConfigs.ApiKey}"); 
            var response = await _httpClient.PostAsJsonAsync(url, input);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<TResponse>(content);
            else
                return default;//JsonSerializer.Deserialize<ErrorResponseDto>(content);
        }

    }
}
