using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Reflection;
using Abp.Runtime.Validation;
using Abp.UI;
using Master.Entity;
using Master.Extension;
using Master.Module.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Master.Module
{
    /// <summary>
    /// 列信息定义
    /// </summary>
    [AutoMap(typeof(ColumnInfo))]
    public class ColumnInfo : FullAuditedEntity, IMustHaveTenant, IExtendableObject, IShouldNormalize, INeedEntityChange
    {
        public virtual string ExtensionData { get; set; }
        public virtual int TenantId { get; set; }
        public virtual int ModuleInfoId { get; set; }
        public virtual ModuleInfo ModuleInfo { get; set; }

        public virtual ColumnTypes ColumnType { get; set; } = ColumnTypes.Text;
        /// <summary>
        /// 是否启用字段权限
        /// </summary>
        public virtual bool EnableFieldPermission { get; set; } = false;
        /// <summary>
        /// 控件类型：如选择类型可以使用下拉框，也可以使用单选
        /// </summary>
        public virtual string ControlFormat { get; set; }

        public virtual int Sort { get; set; }
        /// <summary>
        /// 列标识
        /// </summary>
        public virtual string ColumnKey { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        public virtual string ColumnName { get; set; }
        /// <summary>
        /// 列模板内容
        /// </summary>
        public virtual string Templet { get; set; }
        /// <summary>
        /// 最大上传数量:仅当列类型为文件或图片时有效
        /// </summary>
        public virtual int MaxFileNumber { get; set; } = 1;
        /// <summary>
        /// 是否内置列
        /// </summary>
        public virtual bool IsInterColumn { get; set; }
        /// <summary>
        /// 是否系统列，如创建人，创建日期
        /// </summary>
        public virtual bool IsSystemColumn { get; set; } = false;
        /// <summary>
        /// 格式化字符串如日期:yyyy-MM-dd
        /// </summary>
        public virtual string DisplayFormat { get; set; }
        /// <summary>
        /// 默认值,注意，此值将被进行动态解析，故如果是常量值，请加上引号，如"2010-01-01"
        /// </summary>
        public virtual string DefaultValue { get; set; }
        /// <summary>
        /// 验证规则
        /// </summary>
        public virtual string VerifyRules { get; set; }
        /// <summary>
        /// 自定义呈现
        /// </summary>
        public virtual string Renderer { get; set; }
        /// <summary>
        /// 值路径 OrganizationId
        /// </summary>
        public virtual string ValuePath { get; set; }
        /// <summary>
        /// 显示路径,默认与值路径相同,Organization.DisplayName
        /// </summary>
        public virtual string DisplayPath { get; set; }
        /// <summary>
        /// 关联字典名称
        /// </summary>
        public virtual string DictionaryName { get; set; }
        /// <summary>
        /// 自定义控件
        /// </summary>
        public virtual string CustomizeControl { get; set; }
        /// <summary>
        /// 控件配置信息
        /// </summary>
        public virtual string ControlParameter { get; set; }
        /// <summary>
        /// 是否显示在列表页
        /// </summary>
        public virtual bool IsShownInList { get; set; } = true;
        /// <summary>
        /// 是否显示在添加页
        /// </summary>
        public virtual bool IsShownInAdd { get; set; } = true;
        /// <summary>
        /// 是否显示在修改页
        /// </summary>
        public virtual bool IsShownInEdit { get; set; } = true;
        /// <summary>
        /// 是否显示在批量修改页
        /// </summary>
        public virtual bool IsShownInMultiEdit { get; set; } = true;
        /// <summary>
        /// 是否显示在高级查询页
        /// </summary>
        public virtual bool IsShownInAdvanceSearch { get; set; } = true;
        /// <summary>
        /// 是否显示在查看页
        /// </summary>
        public virtual bool IsShownInView { get; set; } = true;
        /// <summary>
        /// 是否启用排序
        /// </summary>
        public virtual bool IsEnableSort { get; set; }
        /// <summary>
        /// 关联数据来源
        /// </summary>
        public virtual RelativeDataType RelativeDataType { get; set; }
        /// <summary>
        /// 关联数据语句
        /// </summary>
        public virtual string RelativeDataString { get; set; }

        /// <summary>
        /// 输入提示,一般用于前台展示时的提示
        /// </summary>
        [NotMapped]
        public virtual string Tips { get; set; }

        #region 计算属性
        /// <summary>
        /// 最大引用数量
        /// </summary>
        [NotMapped]
        public virtual string MaxReferenceNumber
        {
            get
            {
                return this.GetData<string>("maxReferenceNumber");
            }
            set
            {
                this.SetData("maxReferenceNumber", value);
            }
        }
        /// <summary>
        /// 引用列
        /// </summary>
        [NotMapped]
        public virtual string ReferenceItemTpl
        {
            get
            {
                return this.GetData<string>("referenceItemTpl");
            }
            set
            {
                this.SetData("referenceItemTpl", value);
            }
        }
        /// <summary>
        /// 需要查询显示的列
        /// </summary>
        [NotMapped]
        public virtual string ReferenceSearchColumns
        {
            get
            {
                return this.GetData<string>("referenceSearchColumns");
            }
            set
            {
                this.SetData("referenceSearchColumns", value);
            }
        }
        /// <summary>
        /// 关联数据查询条件
        /// </summary>
        [NotMapped]
        public virtual string ReferenceSearchWhere
        {
            get
            {
                return this.GetData<string>("referenceSearchWhere");
            }
            set
            {
                this.SetData("referenceSearchWhere", value);
            }
        }
        /// <summary>
        /// 是否启用自动完成,只针对text类型有效
        /// </summary>
        [NotMapped]
        public virtual bool EnableAutoComplete
        {
            get
            {
                return this.GetData<bool>("enableAutoComplete");
            }
            set
            {
                this.SetData("enableAutoComplete", value);
            }
        }
        /// <summary>
        /// 是否启用合计行，只针对数字有效
        /// </summary>
        [NotMapped]
        public virtual bool EnableTotalRow
        {
            get
            {
                return this.GetData<bool>("enableTotalRow");
            }
            set
            {
                this.SetData("enableTotalRow", value);
            }
        }
        /// <summary>
        /// 用于前台表格的列排序，加入了固定列修正
        /// </summary>
        [NotMapped]
        public virtual int TableColumnOrder
        {
            get
            {
                var preOrder = 0;
                var fix = this.GetData<string>("fixed");
                if (!string.IsNullOrEmpty(fix))
                {
                    preOrder = fix == "left" ? -1000 : 1000;
                }
                return Sort + preOrder;
            }
        }
        /// <summary>
        /// 此列是否是自定义的属性列
        /// </summary>
        [NotMapped]
        public virtual bool IsPropertyColumn => ValuePath != null && ValuePath.Contains("Property");
        /// <summary>
        /// 此列是否是引用数据列  @[DataKey].[ValuePath]
        /// </summary>
        [NotMapped]
        public virtual bool IsRelativeColumn => ColumnType == ColumnTypes.Reference || (ValuePath != null && ValuePath.StartsWith("@"));
        /// <summary>
        /// 此列是否直接数据列，注：内置直接关联列也是直接数据列，如CustomerId
        /// </summary>
        [NotMapped]
        public virtual bool IsDirectiveColumn => !string.IsNullOrEmpty(ValuePath) && !IsPropertyColumn && !ValuePath.StartsWith("@");
        /// <summary>
        /// 此列是否直接操作列   
        /// </summary>
        [NotMapped]
        public virtual bool IsOperationColumn => IsSystemColumn && ColumnKey == StaticSystemColumns.Operation;
        /// <summary>
        /// 添加权限名
        /// </summary>
        [NotMapped]
        public virtual string ColumnAddPermission => $"Module.{ModuleInfo?.ModuleKey}.Field.{ColumnKey}.Add";
        /// <summary>
        /// 修改权限名
        /// </summary>
        [NotMapped]
        public virtual string ColumnEditPermission => $"Module.{ModuleInfo?.ModuleKey}.Field.{ColumnKey}.Edit";
        /// <summary>
        /// 查看权限名
        /// </summary>
        [NotMapped]
        public virtual string ColumnViewPermission => $"Module.{ModuleInfo?.ModuleKey}.Field.{ColumnKey}.View";
        #endregion

        #region 字段权限
        private bool CheckFieldPerission(string permissionName)
        {
            if (!EnableFieldPermission)
            {
                return true;
            }
            else
            {
                using (var permissionCheckerProvider = IocManager.Instance.ResolveAsDisposable<IPermissionChecker>())
                {
                    return permissionCheckerProvider.Object.IsGranted(permissionName);
                }
            }
        }
        /// <summary>
        /// 对此列是否有添加权限
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckAddPermission()
        {
            return CheckFieldPerission(ColumnAddPermission);
        }
        /// <summary>
        /// 对此列是否有修改权限
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckEditPermission()
        {
            return CheckFieldPerission(ColumnEditPermission);
        }
        /// <summary>
        /// 对此列是否有修改权限
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckViewPermission()
        {
            return CheckFieldPerission(ColumnViewPermission);
        }
        #endregion

        #region 方法

        /// <summary>
        /// 数据初始设定
        /// </summary>
        public virtual void Normalize()
        {
            //todo:完善列信息的预处理
            //例，如果是系统列，则不允许显示在添加修改页
            if (ColumnType == ColumnTypes.Images && string.IsNullOrEmpty(Templet))
            {

            }
            else if (ColumnType == ColumnTypes.Files && string.IsNullOrEmpty(Templet))
            {

            }
            //如果是选择类型的，且dictionaryname是枚举，需要转化枚举类型为字典
            if(ColumnType==ColumnTypes.Select || ColumnType == ColumnTypes.MultiSelect)
            {
                if (!string.IsNullOrEmpty(DictionaryName))
                {
                    using(var ITypeFinderWrapper = IocManager.Instance.ResolveAsDisposable<ITypeFinder>())
                    {
                        var enumType = ITypeFinderWrapper.Object.Find(o => o.FullName == DictionaryName);
                        if (enumType.Length>0)
                        {
                            DictionaryName =Newtonsoft.Json.JsonConvert.SerializeObject( Common.EnumHelper.ToDictionary(enumType[0]));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取列对应的自定义控件
        /// </summary>
        /// <returns></returns>
        public virtual ICustomizeControl GetCustomizeControl()
        {
            if (string.IsNullOrEmpty(CustomizeControl))
            {
                throw new Exception("未找到自定义控件" + ColumnName);
            }
            using (var TypeFinderWrapper = IocManager.Instance.ResolveAsDisposable<ITypeFinder>())
            {
                var controlType = TypeFinderWrapper.Object.Find(o => o.Name == CustomizeControl).FirstOrDefault();
                if (controlType == null)
                {
                    throw new Exception("未找到自定义控件" + CustomizeControl);
                }
                using (var controlWrapper = IocManager.Instance.ResolveAsDisposable(controlType))
                {
                    var control = controlWrapper.Object as ICustomizeControl;
                    return control;
                }
            }

        }
        /// <summary>
        /// 从数据源中获取列对应的强类型数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual object GetStrongTypeValue(IDictionary<string, string> data)
        {
            object obj = null;
            try
            {
                switch (ColumnType)
                {
                    case ColumnTypes.Number:
                        obj = data.GetDataOrException<decimal?>(ColumnKey);
                        break;
                    case ColumnTypes.DateTime:
                        obj = data.GetDataOrException<DateTime?>(ColumnKey);
                        break;
                    case ColumnTypes.Switch:
                        obj = data.GetDataOrException<bool?>(ColumnKey);
                        break;
                    default:
                        obj = data.GetDataOrException<string>(ColumnKey);
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        #endregion

    }

    /// <summary>
    /// 引用数据来源
    /// </summary>
    public enum RelativeDataType
    {
        /// <summary>
        /// 默认来源，即模块内置关联数据来源
        /// </summary>
        Default = 1,
        /// <summary>
        /// 引用其它模块的数据
        /// </summary>
        Module = 2,
        /// <summary>
        /// 自定义SQL语句方式进行引用
        /// </summary>
        CustomSql = 3,
        /// <summary>
        /// url请求方式进行引用
        /// </summary>
        Url = 4
    }
}
