using Abp.Configuration;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class MESSettingNames
    {
        public const string DefaultSourceInner = "MES.DefaultSourceInner";
        //零件重名
        public const string EnableDuplicatePartName = "MES.EnableDuplicatePartName";
        public const string SearchTemplate = "MES.SearchTemplate";
        //必须使用标准名称
        public const string MustUseStandardPartName = "MES.MustUseStandardPartName";
        //标准零件名称
        public const string StandardPartName = "MES.StandardPartName";
        //核算前必须回单审核
        public const string MustReturnFileBeforeCheck = "MES.MustReturnFileBeforeCheck";
        //必须开单审核
        public const string MustConfirmProcess = "MES.MustConfirmProcess";
        //加工开单严格模式
        public const string JGKDStrictMode = "MES.JGKDStrictMode";
        //多零件开单
        public const string EnableMultiPartKaiDan = "MES.EnableMultiPartKaiDan";
        public const string MESCompanySN = "MES.MESCompanySN";
        public const string MESCompanyToken = "MES.MESCompanyToken";
        //开单理由
        public const string JGKDReason = "MES.JGKDReason";
    }
    public class MESSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var group = new SettingDefinitionGroup("MES", L("生产"));
            var group2= new SettingDefinitionGroup("Cloud", L("云平台"));
            return new SettingDefinition[]
            {
                new SettingDefinition(MESSettingNames.DefaultSourceInner, "false",L("开单来源"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Select,ControlFormat="radio", Tips="设置加工开单默认厂内或是厂外",DictionaryName="{\"true\":\"厂内\",\"false\":\"厂外\"}"}),

                new SettingDefinition(MESSettingNames.JGKDStrictMode, "false",L("严格开单"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Switch, Tips="严格模式下模具编号、加工点、工序只能选择不能输入"}),

                new SettingDefinition(MESSettingNames.EnableDuplicatePartName, "false",L("零件重名"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Switch,Tips="若设为是,则同一副模具内允许出现重名零件"}),

                new SettingDefinition(MESSettingNames.MustUseStandardPartName, "false",L("零件名称标准化"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Switch,Tips="若设为是,则必须使用标准零件名称库中的零件名称"}),

                new SettingDefinition(MESSettingNames.MustConfirmProcess, "false",L("强制开单审核"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Switch,Tips="若设为是,则必须经过开单审核的加工单才能进行打印报工"}),

                new SettingDefinition(MESSettingNames.MustReturnFileBeforeCheck, "true",L("强制回单审核"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Switch,Tips="若设为是,则仅已回单审核过的加工单可以进行核算"}),

                new SettingDefinition(MESSettingNames.EnableMultiPartKaiDan, "false",L("多零件开单"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Switch,Tips="若设为是,则可以进行多零件批量开单"}),

                new SettingDefinition(MESSettingNames.StandardPartName, "",L("标准零件库"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text,Renderer="lay-standardpart"}),

                 new SettingDefinition(MESSettingNames.JGKDReason, "",L("开单理由"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text,Renderer="lay-JGKDReason"}),

                 new SettingDefinition(MESSettingNames.SearchTemplate, "",L("搜索模板"),null, scopes: SettingScopes.User , isVisibleToClients: false),

                new SettingDefinition(MESSettingNames.MESCompanySN, "",L("企业编号"),group2, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
                new SettingDefinition(MESSettingNames.MESCompanyToken, "",L("企业令牌"),group2, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text}),
                //new SettingDefinition(MESSettingNames.CompanyToken, "",L("企业令牌"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true)
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, MasterConsts.LocalizationSourceName);
        }
    }
}
