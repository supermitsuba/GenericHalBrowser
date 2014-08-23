using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericHalHelper.Models
{
    public class Link
    {
        public string Name { get; set; }
        public string Href { get; set; }
        public string IsTemplate { get; set; }

        public string Type { get; set; }

        public string Deprecation { get; set; }

        public string Profile { get; set; }

        public string Title { get; set; }

        public string Hreflang { get; set; }
    }
}
