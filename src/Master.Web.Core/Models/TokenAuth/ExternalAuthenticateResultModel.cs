using System.Collections.Generic;

namespace Master.Models.TokenAuth
{
    public class ExternalAuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public bool WaitingForActivation { get; set; }

        public IEnumerable<string> GrantedPermissions { get; set; }
    }
}
