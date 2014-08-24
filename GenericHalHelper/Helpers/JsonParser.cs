using GenericHalHelper.Exceptions;
using GenericHalHelper.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericHalHelper.Helpers
{
    internal static class JsonParser
    {
        public static HalObject ParseJson(string json)
        {
            var obj = JObject.Parse(json);
            var properties = obj.Properties()
                                .Where(p => p.Name != "_embedded" &&
                                       p.Name != "_links");
            
            var dictProperties = ParseProperties(properties, obj);

            var dictEmbedded = ParseEmbedded(obj["_embedded"]);

            var dictLinks = ParseLinks(obj["_links"]);

            return new HalObject(dictProperties, dictEmbedded, dictLinks);
        }


        private static IDictionary<string, IList<HalObject>> ParseEmbedded(JToken embedded)
        {
            var result = new Dictionary<string, IList<HalObject>>();
            if (embedded != null && embedded.Any())
            {
                var e = embedded.Value<JObject>();
                var q = e.Properties().Select(p => p.Name);

                foreach (var propertyName in q)
                {
                    var linksInRel = e[propertyName].Value<JArray>();
                    List<HalObject> objects = new List<HalObject>();
                    foreach (JObject item in linksInRel)
                    {
                        objects.Add(JsonParser.ParseJson(item.ToString()));
                    }
                    result.Add(propertyName, objects);
                }
            }

            return result;
        }

        public static IDictionary<string, string> ParseProperties(IEnumerable<JProperty> properties, JObject obj)
        {
            var result = new Dictionary<string, string>();
            if (properties != null && properties.Any())
            {
                foreach (var p in properties)
                {
                    result.Add(p.Name, (string)p.Value);
                }
            }

            return result;
        }
        
        public static IDictionary<string, IList<Link>> ParseLinks(JToken links)
        {
            var result = new Dictionary<string, IList<Link>>();
            if (links != null && links.Any())
            {
                foreach (JProperty l in links)
                {
                    var rel = l.Name;
                    if (l.Value.Type == JTokenType.Object)
                    {
                        var linksInRel = (l.Value as JObject);
                        result.Add(rel,
                            new List<Link>() { 
                                CreateLink(linksInRel)
                            });
                    }
                    else if (l.Value.Type == JTokenType.Array)
                    {
                        var linksInRel = (l.Value as JArray);
                        foreach (JObject item in linksInRel)
                        {
                            result.Add(rel,
                                new List<Link>() { 
                                    CreateLink(item)
                            });
                        }
                    }
                    else
                    {
                        throw new InvalidJsonLinkStructureException(string.Format("Could not deserialize link: {0}", rel));
                    }
                }
            }

            return result;
        }

        private static Link CreateLink(JObject linksInRel)
        {
            return new Link()
            {
                Name = linksInRel.Lookup("name"),
                Href = linksInRel.Lookup("Href"),
                IsTemplate = linksInRel.Lookup("templated"),
                Deprecation = linksInRel.Lookup("deprecation"),
                Hreflang = linksInRel.Lookup("hreflang"),
                Profile = linksInRel.Lookup("profile"),
                Title = linksInRel.Lookup("title"),
                Type = linksInRel.Lookup("type"),
            };
        }

    }
}
