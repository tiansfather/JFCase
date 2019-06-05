using Abp.Configuration;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public static class SettingNames
    {
        public const string MenuSetting = "Menu";
        public const string SoftTitle = "App.SoftTitle";
        public const string login_lockoutCount = "login_lockoutTimes";
        public const string login_lockoutDuration = "login_lockoutDuration";
        public const string maxWorkbenchCaseCount = "maxWorkbenchCaseCount";
        public const string receiveMailAddress = "receiveMailAddress";

        public const string mail_smtpServer = "mail_smtpServer";
        public const string mail_ssl = "mail_ssl";
        public const string mail_username = "mail_username";
        public const string mail_pwd = "mail_pwd";
        public const string mail_nickname = "mail_nickname";
        public const string mail_fromname = "mail_fromname";
    }
    public class MasterSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            //var interGroup = new SettingDefinitionGroup("InterSetting", L("内部设置"));
            //group设为null则不在设置页面中出现
            var menuSettingDefinition = new SettingDefinition(SettingNames.MenuSetting, "", L("菜单"), group: null, scopes: SettingScopes.Tenant | SettingScopes.User);


            var group = new SettingDefinitionGroup("Core", L("基本设置"));
            var group2 = new SettingDefinitionGroup("Login", L("登录"));
            var group3 = new SettingDefinitionGroup("Email", L("邮件"));
            return new SettingDefinition[]
            {
                menuSettingDefinition,
                new SettingDefinition(SettingNames.SoftTitle, "简法案例系统",L("系统标题"),group, scopes: SettingScopes.Application , isVisibleToClients: true),
                new SettingDefinition(SettingNames.login_lockoutCount, "6",L("失败次数"),group2, scopes: SettingScopes.Application,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Number,Tips="同一用户名登录失败超过此数字将被锁定登录"}),
                new SettingDefinition(SettingNames.login_lockoutDuration, "30",L("锁定时长"),group2, scopes: SettingScopes.Application,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Number,Tips="单位分钟"}),
                new SettingDefinition(SettingNames.maxWorkbenchCaseCount, "10",L("工作台总数"),group, scopes: SettingScopes.Application , isVisibleToClients: true),
                new SettingDefinition(SettingNames.receiveMailAddress, "",L("接收邮件地址"),group, scopes: SettingScopes.Application , isVisibleToClients: true),

                new SettingDefinition(SettingNames.mail_smtpServer, "",L("SMTP服务器"),group3, scopes: SettingScopes.Application ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),

                new SettingDefinition(SettingNames.mail_ssl, "false",L("SSL"),group3, scopes: SettingScopes.Application ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Switch}),

                new SettingDefinition(SettingNames.mail_username, "",L("用户名"),group3, scopes: SettingScopes.Application , customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
                new SettingDefinition(SettingNames.mail_pwd, "",L("密码"),group3, scopes: SettingScopes.Application , customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
                new SettingDefinition(SettingNames.mail_nickname, "",L("昵称"),group3, scopes: SettingScopes.Application ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
                new SettingDefinition(SettingNames.mail_fromname, "",L("发件人"),group3, scopes: SettingScopes.Application , isVisibleToClients:true,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, MasterConsts.LocalizationSourceName);
        }
    }
}
