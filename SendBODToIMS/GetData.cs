
using Newtonsoft.Json;
using SendBODToIMS;
using System;
using System.Collections.Generic;

namespace CreateCompanyDivision
{
    public class GetData : apiService
    {
        public string PostIMS(IONAPIFile aCredentials, IMS aIMS)
        {
            string result = null;
            string separator = "";
            string baseAPI = "IONSERVICES/api/ion/messaging/service/v2/message";

            string request = aCredentials.getIONAPIUrl() + separator + baseAPI;    //Uri.EscapeDataString(apiCall + query);

            result = callService(aCredentials, new Uri(request), JsonConvert.SerializeObject(aIMS));

            return (result);
        }

    }
}
