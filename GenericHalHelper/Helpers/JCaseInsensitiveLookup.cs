using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericHalHelper.Helpers
{
    internal static class JCaseInsensitiveLookup
    {
        public static string Lookup(this JObject token, string propertyName)
        {
            var result = token.Properties()
                              .Where(p => p.Name.ToLower() == propertyName.ToLower())
                              .FirstOrDefault();
            if(result != null && result.Any())
            {
                return (string)result.Value;
            }
            else
            {
                return null;
            }
        }
    }
}
