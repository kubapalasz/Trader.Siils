using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SLYEx.API.Dummy.Controllers
{
    public class BalanceController : ApiController
    {   
        public string Get()
        {
            var currentBalanceJson = JsonConvert.SerializeObject(BalanceContainer.Current,
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});

            return currentBalanceJson;
        }
    }
}
