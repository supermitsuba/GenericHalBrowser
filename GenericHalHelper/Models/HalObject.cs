using GenericHalHelper.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GenericHalHelper.Models
{
    public class HalObject
    {
        public HalObject(IDictionary<string, string> dataProperties, 
            IDictionary<string, IList<HalObject>> embeddedResources,
            IDictionary<string, IList<Link>> links)
        {
            DataProperties = dataProperties;
            EmbeddedResources = embeddedResources;
            Links = links;
        }

        public IDictionary<string, string> DataProperties { get; private set; }

        public IDictionary<string, IList<HalObject>> EmbeddedResources { get; private set; }

        public IDictionary<string, IList<Link>> Links { get; private set; }
    }
}
