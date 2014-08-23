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
            var jsonString = _client.GetStringAsync(relativePath);
            jsonString.Wait();
            var o = JObject.Parse(jsonString.Result);
            var embedded = o["_embedded"];
            var links = o["_links"];
            var properties = o.Properties()
                              .Where(p => p.Name != "_embedded" &&
                                     p.Name != "_links");

            var dictProperties = new Dictionary<string, string>();
            if (properties.Any())
            {
                foreach (var p in properties)
                {
                    dictProperties.Add(p.Name, o[p].ToString());
                }
            }

            var dictEmbedded = new Dictionary<string, HalObject>();

            var dictLinks = new Dictionary<string, Link>();
            if (links.Any())
            {
                foreach (var link in links)
                {
                    var l1 = JObject.Parse(string.Format("{{ {0} }}", link.ToString()))
                                    .Properties();
                    if (l1.Any())
                    {
                        foreach (var p in l1)
                        {
                            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(p.Values().ToString());
                            dictLinks.Add(p.Name,
                                new Link()
                                {
                                    Name = dict.ContainsKey("name") ? dict["name"] : "",
                                    Href = dict.ContainsKey("href") ? dict["href"] : "",
                                    IsTemplate = dict.ContainsKey("templated") ? dict["templated"] : "",
                                    Deprecation = dict.ContainsKey("deprecation") ? dict["deprecation"] : "",
                                    Hreflang = dict.ContainsKey("hreflang") ? dict["hreflang"] : "",
                                    Profile = dict.ContainsKey("profile") ? dict["profile"] : "",
                                    Title = dict.ContainsKey("title") ? dict["title"] : "",
                                    Type = dict.ContainsKey("type") ? dict["type"] : ""
                                }
                            );
                        }
                    }
                }
            }

            return new HalObject(dictProperties, dictEmbedded, dictLinks);
        }

        public HalObject Get(string relativePath, object uriParamters)
        {
            throw new NotImplementedException();
        }

        public HalObject Get(Models.Link linkToNextState)
        {
            throw new NotImplementedException();
        }

        public HalObject Get(Models.Link linkToNextState, object uriParameters)
        {
            throw new NotImplementedException();
        }
    }
}
