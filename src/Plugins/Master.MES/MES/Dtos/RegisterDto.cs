using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 微信端提交的注册信息
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// 公司名
        /// </summary>
        [Required]
        public string CompanyName { get; set; }
        /// <summary>
        /// 账套名
        /// </summary>
        [Required]
        public string TenancyName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        public string Mobile { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 邀请账套id
        /// </summary>
        public int? Inviter { get; set; }
        /// <summary>
        /// 对应邀请方的往来单位名
        /// </summary>
        public string InviterUnitName { get; set; }
    }
}
