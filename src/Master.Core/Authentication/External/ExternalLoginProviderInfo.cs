using System;

namespace Master.Authentication.External
{
    public class ExternalLoginProviderInfo
    {
        public string Name { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public Type ProviderApiType { get; set; }
        public string AuthUrl { get; set; }
        public string BindUrl { get; set; }
        public object Data { get; set; }

        public ExternalLoginProviderInfo(string name, string clientId, string clientSecret, Type providerApiType,string authUrl="",string bindUrl="",object data=null)
        {
            Name = name;
            ClientId = clientId;
            ClientSecret = clientSecret;
            ProviderApiType = providerApiType;
            AuthUrl = authUrl;
            BindUrl = bindUrl;
            Data = data;
            
        }
    }
}
