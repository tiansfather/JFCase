using Abp.AutoMapper;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Module
{
    [AutoMap(typeof(ModuleButton))]
    public class ModuleButton : BaseFullEntityWithTenant
    {
        [Required]
        public virtual int ModuleInfoId { get; set; }
        public virtual ModuleInfo ModuleInfo { get; set; }
        public virtual string ButtonKey { get; set; }
        public virtual string ButtonName { get; set; }
        /// <summary>
        /// 前台样式类
        /// </summary>
        public virtual string ButtonClass { get; set; }
        /// <summary>
        /// 对话框标题模板
        /// </summary>
        public string TitleTemplet { get; set; }
        /// <summary>
        /// 行为类型:异步提交、表单
        /// </summary>
        public virtual ButtonActionType ButtonActionType { get; set; } = ButtonActionType.Ajax;
        /// <summary>
        /// 按钮类型:单行按钮、多行按钮、通用按钮
        /// </summary>
        public virtual ButtonType ButtonType { get; set; }
        /// <summary>
        /// 动作地址：请求url或请求的js函数
        /// </summary>
        public virtual string ButtonActionUrl { get; set; }
        /// <summary>
        /// 动作参数
        /// </summary>
        public virtual string ButtonActionParam { get; set; }
        public virtual string ConfirmMsg { get; set; }
        public virtual string ButtonScript { get; set; }
        public int Sort { get; set; }
        public virtual bool IsEnabled { get; set; } = true;
        /// <summary>
        /// 是否需要权限
        /// </summary>
        public virtual bool RequirePermission { get; set; } = true;
        /// <summary>
        /// 特殊条件控制，如已离职员工不出现离职按钮
        /// </summary>
        public virtual string ClientShowCondition { get; set; }

        /// <summary>
        /// 动作的权限名称
        /// </summary>
        [NotMapped]
        public virtual string ButtonPermissionName => $"Module.{ModuleInfo?.ModuleKey}.Button.{ButtonKey}";

    }

    [Flags]
    public enum ButtonType
    {
        /// <summary>
        /// 单行
        /// </summary>
        ForSingleRow = 1,
        /// <summary>
        /// 多行
        /// </summary>
        ForSelectedRows = 2,
        /// <summary>
        /// 顶部
        /// </summary>
        ForNoneRow = 4
    }

    public enum ButtonActionType
    {
        /// <summary>
        /// 资源标记,一般用于代表权限
        /// </summary>
        Resource = 0,
        /// <summary>
        /// 异步提交
        /// </summary>
        Ajax = 1,
        /// <summary>
        /// 表单
        /// </summary>
        Form = 2,
        /// <summary>
        /// 打开标签页
        /// </summary>
        Tab = 3,
        /// <summary>
        /// 直接执行函数
        /// </summary>
        Func = 4,
    }
}
