using GenericHalHelper.Exceptions;
using GenericHalHelper.Helpers;
using GenericHalHelper.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenericHalHelper
{
    public class HalClient : IHalClient
    {
        private HttpClient _client;

        public HalClient(HttpClient client)
        {
            _client = client;
        }

        public HalObject Get(string relativePath)
        {
            var jsonString = _client.GetAsync(relativePath);
            jsonString.Wait();
            var stringTask = jsonString.Result.Content.ReadAsStringAsync();
            if (jsonString.Result.IsSuccessStatusCode)
            {
                return JsonParser.ParseJson(stringTask.Result);
            }
            else
            {
                throw new HttpRequestException(stringTask.Result);
            }
        }

        public HalObject Get(string relativePath, object uriParamters)
        {
            throw new NotImplementedException();
        }

        public HalObject Get(Link linkToNextState)
        {
            return Get(linkToNextState.Href);
        }

        public HalObject Get(Link linkToNextState, object uriParameters)
        {
            throw new NotImplementedException();
        }
    }
}
