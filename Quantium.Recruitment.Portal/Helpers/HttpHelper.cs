using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Quantium.Recruitment.Portal.Helpers
{
    public interface IHttpHelper
    {
        HttpResponseMessage Post(string uri, object data);
        HttpResponseMessage GetData(string uri);
        HttpResponseMessage Post(string uri, Stream fileStream);
    }

    public class HttpHelper : IHttpHelper
    {
        private readonly IOptions<ConfigurationOptions> _configOptions;

        public HttpHelper(IOptions<ConfigurationOptions> configOptions)
        {
            _configOptions = configOptions;
        }

        public HttpResponseMessage Post(string uri, object data)
        {
            var client = GetClient();
            var jsonData = JsonConvert.SerializeObject(data);

            HttpContent contentPost = new StringContent(jsonData, Encoding.UTF8, "application/json");

            return client.PostAsync(uri, contentPost).Result;
        }

        public HttpResponseMessage Post(string uri, Stream fileStream)
        {
            var client = GetClient();

            //new MultipartContent();
            //var multiContent = new MultipartFormDataContent();
            //var stream = file.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            //streamContent.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //multiContent.Add(streamContent);
            return client.PostAsync(uri, streamContent).Result;
        }

        public HttpResponseMessage GetData(string uri)
        {
            var client = GetClient();
            return client.GetAsync(uri).Result;
        }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_configOptions.Value.ApiServiceEndpoint);
            return client;
        }
    }
}
