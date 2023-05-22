using Newtonsoft.Json;
using WebApi.Enums;
using WebApi.Utilities;

namespace WebApi.Models
{
    public class ApiResult
    {
        public ApiResultCode Code { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
        public string Message { get; set; }

        public ApiResult()
        {
            Code = ApiResultCode.Ready;
            Data = null;
            Message = EnumHelper.GetDescription((ApiResultCode)Code);
        }
    }
}
