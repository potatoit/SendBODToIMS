using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendBODToIMS
{
    public class IMSResponse
    {
        public string Error { get; set; }
        public string Result { get; set; }

        public string StatusCode { get; set; }
    }
}
