using Abp.AutoMapper;
using Master.Authentication.External;

namespace Master.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string AuthUrl { get; set; }
        public object Data { get; set; }
    }
}
