using GenericHalHelper;
using GenericHalHelper.Helpers;
using System;
using System.Linq;
using System.Net.Http;

namespace GenericHalBrowser
{
    class Program
    {
        static void Main(string[] args)
        {
            using(HttpClient client = new HttpClient())
            {
                var HalMediaType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.hal+json");
                client.DefaultRequestHeaders.Accept.Add(HalMediaType);
                client.BaseAddress = new Uri("http://www.fbombcode.com");
                HalClient hal = new HalClient(client);
                var o = hal.Get("api");

                var link2 = o.Links["fbomb:articles"][0];
                var o2 = hal.Get(link2);

                var data = o2.EmbeddedResources["fbomb:articles"]
                             .Where(p => p.DataProperties["Title"] == "My experiences with Hypermedia API");

                int i = 0;
                while (data != null && data.Count() == 0)
                {
                    foreach (var item in o2.EmbeddedResources["fbomb:articles"])
                    {
                        i++;
                        Console.WriteLine("Category: {0}", item.DataProperties["Title"]);
                    }
                    if (!o2.Links.ContainsKey(RelConstants.next))
                    {
                        Console.WriteLine("There is no article category by that name! Looked through {0} articles.", i);
                        Console.ReadKey();
                        return;
                    }

                    o2 = hal.Get(o2.Links[RelConstants.next][0]);
                    data = o2.EmbeddedResources["fbomb:articles"]
                             .Where(p => p.DataProperties["Title"] == "My experiences with Hypermedia API");
                }                

                var link3 = data.First().Links["fbomb:articleById"][0];
                var o3 = hal.Get(link3);

                var link4 = o3.Links["fbomb:commentsByArticle"][0];
                var o4 = hal.Get(link4);

                Console.WriteLine("For article: {0}, you have {1} comments!", o3.DataProperties["Title"], o4.EmbeddedResources["fbomb:comments"].Count);
                Console.ReadKey();
            }
        }
    }
}
