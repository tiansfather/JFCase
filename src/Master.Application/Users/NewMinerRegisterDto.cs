using Abp.AutoMapper;
using Master.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Master.Users
{
    /// <summary>
    /// 矿工注册
    /// </summary>
    [AutoMap(typeof(NewMiner))]
    public class NewMinerRegisterDto
    {
        public string OpenId { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avata { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 律师事务所
        /// </summary>
        [Required]
        public string WorkLocation { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
