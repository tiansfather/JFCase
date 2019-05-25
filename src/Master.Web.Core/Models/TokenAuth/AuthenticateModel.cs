using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Master.Models.TokenAuth
{
    /// <summary>
    /// Token提交模型
    /// </summary>
    public class AuthenticateModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string TenancyName { get; set; }
        [Required]
        public string VerifyCode { get; set; }
    }
}
