using GenericHalHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
                client.BaseAddress = new Uri("http://fbombcode.com/");
                HalClient hal = new HalClient(client);
                var o = hal.Get("api");
            }
        }
    }
}
