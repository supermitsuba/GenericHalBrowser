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

            var dictLinks = new Dictionary<string, IList<Link>>();
            if (links.Any())
            {
                foreach (JProperty l in links)
                {
                    var rel = l.Name;
                    if(l.Value.Type == JTokenType.Object)
                    {
                        var linksInRel = (l.Value as JObject);
                        dictLinks.Add(rel, 
                            new List<Link>() { 
                                new Link()
                                {
                                    Name = linksInRel.Lookup("name"),
                                    Href = linksInRel.Lookup("Href"),
                                    IsTemplate = linksInRel.Lookup("templated"),
                                    Deprecation = linksInRel.Lookup("deprecation"),
                                    Hreflang = linksInRel.Lookup("hreflang"),
                                    Profile = linksInRel.Lookup("profile"),
                                    Title = linksInRel.Lookup("title"),
                                    Type = linksInRel.Lookup("type"),
                                }
                            });
                    }
                    else if (l.Value.Type == JTokenType.Array)
                    {
                        var linksInRel = (l.Value as JArray);
                        foreach (JObject item in linksInRel)
                        {
                            dictLinks.Add(rel,
                                new List<Link>() { 
                                new Link()
                                {
                                    Name = item.Lookup("name"),
                                    Href = item.Lookup("Href"),
                                    IsTemplate = item.Lookup("templated"),
                                    Deprecation = item.Lookup("deprecation"),
                                    Hreflang = item.Lookup("hreflang"),
                                    Profile = item.Lookup("profile"),
                                    Title = item.Lookup("title"),
                                    Type = item.Lookup("type"),
                                }
                            });
                        }
                    }
                    else
                    {
                        throw new InvalidJsonLinkStructureException(string.Format("Could not deserialize link: {0}", rel));
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
