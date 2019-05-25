using Abp.Configuration;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class WebSettingNames
    {
        public const string mail_smtpServer = "mail_smtpServer";
        public const string mail_ssl = "mail_ssl";
        public const string mail_username = "mail_username";
        public const string mail_pwd = "mail_pwd";
        public const string mail_nickname = "mail_nickname";
        public const string mail_fromname = "mail_fromname";
    }
    public class WebSettingProvider : Abp.Configuration.SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var group = new SettingDefinitionGroup("Email", L("邮件"));
            return new SettingDefinition[]
            {
                new SettingDefinition(WebSettingNames.mail_smtpServer, "",L("SMTP服务器"),group, scopes: SettingScopes.Tenant ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),

                new SettingDefinition(WebSettingNames.mail_ssl, "false",L("SSL"),group, scopes: SettingScopes.Tenant ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Switch}),

                new SettingDefinition(WebSettingNames.mail_username, "",L("用户名"),group, scopes: SettingScopes.Tenant , customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
                new SettingDefinition(WebSettingNames.mail_pwd, "",L("密码"),group, scopes: SettingScopes.Tenant , customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
                new SettingDefinition(WebSettingNames.mail_nickname, "",L("昵称"),group, scopes: SettingScopes.Tenant ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
                new SettingDefinition(WebSettingNames.mail_fromname, "",L("发件人"),group, scopes: SettingScopes.Tenant , customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, MasterConsts.LocalizationSourceName);
        }
    }
}
