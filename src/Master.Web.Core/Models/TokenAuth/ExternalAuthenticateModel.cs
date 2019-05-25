using System.ComponentModel.DataAnnotations;

namespace Master.Models.TokenAuth
{
    public class ExternalAuthenticateModel
    {
        [Required]
        public string AuthProvider { get; set; }

        [Required]
        public string ProviderKey { get; set; }

        public string ProviderAccessCode { get; set; }
        public string ClientInfo { get; set; }
    }
}
