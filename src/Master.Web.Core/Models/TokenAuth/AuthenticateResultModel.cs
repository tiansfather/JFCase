using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }

        public IEnumerable<string> GrantedPermissions { get; set; }
    }
}
