using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendBODToIMS
{
    public class IMS
    {
        public string documentName { get; set; }
        public string messageId { get; set; }
        public string fromLogicalId { get; set; }
        public string toLogicalId { get; set; }
        public doc document { get; set; }
    }

    public class doc
    {
        public string value { get; set; }
        public string encoding { get; set; } = "NONE";
        public string characterSet { get; set; } = "UTF-8";

    }
}
