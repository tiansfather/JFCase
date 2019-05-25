using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Reflection;
using Abp.Reflection.Extensions;
using Master.Authentication;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Master.Module
{
    public class ModuleInfo : FullAuditedEntity, IMustHaveTenant, IExtendableObject
    {
        public string ExtensionData { get; set; }
        public int TenantId { get; set; }
        /// <summary>
        /// 是否内置模块
        /// </summary>
        public bool IsInterModule { get; set; } = false;
        /// <summary>
        /// 需求特性
        /// </summary>
        public string RequiredFeature { get; set; }
        /// <summary>
        /// 模块识别标记
        /// </summary>
        public string ModuleKey { get; set; }
        /// <summary>
        /// 对应实体类全名
        /// </summary>
        public string EntityFullName { get; set; } = typeof(ModuleData).FullName;
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 默认每页数量
        /// </summary>
        public int DefaultLimit { get; set; } = 10;
        /// <summary>
        /// 每页数量可选值
        /// </summary>
        public string Limits { get; set; } = "[10,15,50,100]";
        /// <summary>
        /// 默认排序字段
        /// </summary>
        public string SortField { get; set; } = "Id";
        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType SortType { get; set; } = SortType.Desc;
        [ForeignKey("CreatorUserId")]
        public virtual User CreatorUser { get; set; }
        [ForeignKey("LastModifierUserId")]
        public virtual User LastModifierUser { get; set; }
        [ForeignKey("DeleterUserId")]
        public virtual User DeleterUser { get; set; }
        public virtual ICollection<ColumnInfo> ColumnInfos { get; set; }
        public virtual ICollection<ModuleButton> Buttons { get; set; }

        

        #region 方法
        /// <summary>
        /// 模块所在的插件名称，用于js中调用,abp.services.[app]
        /// </summary>
        /// <returns></returns>
        public virtual string GetPluginName()
        {
            var pluginName = this.GetData<string>("PluginName");
            if (string.IsNullOrEmpty(pluginName) || pluginName == "core" || pluginName=="mes") { pluginName = "app"; }
            return pluginName;
        }

        #region 列的相关方法

        #region 1 附加列的方法 
        /// <summary>
        /// 附加列单个
        /// </summary>
        /// <param name="columnInfo"></param>
        public virtual void AddColumnInfo(ColumnInfo columnInfo)
        {
            #region 原始列的预处理
            if (ColumnInfos == null)
            {
                ColumnInfos = new List<ColumnInfo>();
            }
            #endregion

            //判断是否有对应的列
            if (!HaveColumn(columnInfo.ColumnKey))
            {
                columnInfo.TenantId = TenantId;
                ColumnInfos.Add(columnInfo);
            }
        }

        /// <summary>
        /// 附加列多个
        /// </summary>
        /// <param name="columnInfos"></param>
        public virtual void AddColumnInfo(List<ColumnInfo> columnInfos)
        {
            #region 原始列的预处理
            if (ColumnInfos == null)
            {
                ColumnInfos = new List<ColumnInfo>();
            }
            #endregion

            //附加基础列
            if (ColumnInfos.Count == 0)
            {
                foreach (var columnInfo in columnInfos)
                {
                    columnInfo.TenantId = TenantId;
                    ColumnInfos.Add(columnInfo);
                }
            }
            else
            {
                foreach (var columnInfo in columnInfos)
                {
                    //判断是否有对应的列
                    if (!HaveColumn(columnInfo.ColumnKey))
                    {
                        columnInfo.TenantId = TenantId;
                        ColumnInfos.Add(columnInfo);
                    }
                }
            }
        }
        #endregion

        #region 2 判断列是否重复 return bool
        /// <summary>
        /// 判定是否有了对应列
        /// </summary>
        /// <param name="columnKey"></param>
        /// <returns></returns>
        public bool HaveColumn(string columnKey) => ColumnInfos != null && ColumnInfos.Count(o => o.ColumnKey == columnKey) > 0;
        //{
        //    bool have = false;
        //    if (ColumnInfos != null)
        //    {
        //        if (ColumnInfos.Count > 0)
        //        {
        //            foreach (var ColumnInfo in ColumnInfos)
        //            {
        //                if (ColumnInfo.ColumnKey == columnKey)
        //                {
        //                    have = true;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return have;
        //}
        #endregion 

        #region 列构建的提供方法 1 构建默认列可以被重写
        /// <summary>
        /// 构建默认列(可被重写)
        /// </summary>
        public virtual void MakeDefaultColumns()
        {
            #region 默认列
            #region 原始列的预处理
            if (ColumnInfos == null)
            {
                ColumnInfos = new List<ColumnInfo>();
            }
            #endregion

            #region 基础列的生名
            List<ColumnInfo> BaseColumnInfos = new List<ColumnInfo>();
            BaseColumnInfos.Add(new ColumnInfo()
            {
                ColumnKey = StaticSystemColumns.Creator,
                ColumnName = "创建人",
                ValuePath = "CreatorUser.Name",
                IsInterColumn = true,
                IsSystemColumn = true,
                IsShownInAdd = false,
                IsShownInEdit = false,
                ColumnType = ColumnTypes.Text,
                Sort=999
            });
            BaseColumnInfos.Add(new ColumnInfo()
            {
                ColumnKey = StaticSystemColumns.CreationTime,
                ColumnName = "创建时间",
                ValuePath = StaticSystemColumns.CreationTime,
                IsInterColumn = true,
                IsSystemColumn = true,
                IsShownInAdd = false,
                IsShownInEdit = false,
                IsEnableSort=true,
                ColumnType = ColumnTypes.DateTime,
                DisplayFormat = "yyyy-MM-dd HH:mm:ss",
                ControlFormat = "datetime",
                Sort = 999
            });
            BaseColumnInfos.Add(new ColumnInfo()
            {
                ColumnKey = StaticSystemColumns.Modifier,
                ColumnName = "修改人",
                ValuePath = "LastModifierUser.Name",
                IsInterColumn = true,
                IsSystemColumn = true,
                IsShownInAdd = false,
                IsShownInEdit = false,
                ColumnType = ColumnTypes.Text,
                Sort = 999
            });
            BaseColumnInfos.Add(new ColumnInfo()
            {
                ColumnKey = StaticSystemColumns.ModifyTime,
                ColumnName = "修改时间",
                ValuePath = StaticSystemColumns.ModifyTime,
                IsInterColumn = true,
                IsSystemColumn = true,
                IsShownInAdd = false,
                IsShownInEdit = false,
                ColumnType = ColumnTypes.DateTime,
                DisplayFormat = "yyyy-MM-dd HH:mm:ss",
                ControlFormat = "datetime",
                Sort = 999
            });
            var operationColumnInfo = new ColumnInfo()
            {
                ColumnKey = StaticSystemColumns.Operation,
                ColumnName = "操作",
                IsInterColumn = true,
                IsSystemColumn = true,
                ColumnType = ColumnTypes.System,
                IsShownInAdd = false,
                IsShownInEdit = false,
                IsShownInView = false,
                Sort = 1000
            };
            operationColumnInfo.SetData("fixed", "right");
            BaseColumnInfos.Add(operationColumnInfo);

            #endregion

            //附加基础列
            AddColumnInfo(BaseColumnInfos);
            #endregion
        }
        #endregion

        #region 列构建的提供方法 2 通过类反射获取列信息,建立列信息
        /// <summary>
        /// 通过类反射获取列信息,建立列信息
        /// </summary>
        /// <param name="type"></param>
        public virtual void CreateInterColumnsFromType(Type type)
        {
            #region 原始列的预处理
            if (ColumnInfos == null)
            {
                ColumnInfos = new List<ColumnInfo>();
            }
            #endregion

            var result = new List<ColumnInfo>();
            //通过属性特性生成列
            foreach (var property in type.GetProperties())
            {
                var interColumnInfo = property.GetSingleAttributeOrNull<InterColumnAttribute>();
                if (interColumnInfo != null)
                {
                    var tempColumnInfo = interColumnInfo.BuildColumnInfo(property);
                    //tempColumnInfo.ColumnKey = property.Name;
                    //tempColumnInfo.ValuePath =string.IsNullOrEmpty(tempColumnInfo.ValuePath)?property.Name:tempColumnInfo.ValuePath;
                    if (!HaveColumn(tempColumnInfo.ColumnKey))
                    {
                        tempColumnInfo.TenantId = TenantId;
                        ColumnInfos.Add(tempColumnInfo);
                    }
                }
            }
        }
        #endregion

        #region 列构建的提供方法 3 通过类反射获取自定义的列信息,建立列信息
        /// <summary>
        /// 构建自定义列
        /// </summary>
        /// <param name="id"></param>
        public virtual void CreateCustomizeColumns(string type)
        {
            using(var typeFinderWrapper = IocManager.Instance.ResolveAsDisposable<ITypeFinder>())
            {
                //var types = typeFinderWrapper.Object.Find(o => o.FullName == "Master.EntityFrameworkCore.Seed.BaseData." + type + "BaseModules");
                var types = typeFinderWrapper.Object.Find(o => o.FullName.EndsWith("." + type + "BaseModules"));
                foreach (var t in types)
                {
                    //增加列
                    var method = t.GetMethod("GetColumnInfos");
                    object obj = Activator.CreateInstance(t);
                    var columnInfos = method.Invoke(obj, null) as List<ColumnInfo>;
                    AddColumnInfo(columnInfos);

                    //补齐列
                    var method2 = t.GetMethod("SetColumnInfosMoreData");
                    method2.Invoke(obj, new object[] { ColumnInfos });
                }
            }
            //Type t = Type.GetType("Master.EntityFrameworkCore.Seed.BaseData." + type + "BaseModules, Master.EntityFrameworkCore");
            //if (t != null)
            //{
            //    //增加列
            //    var method = t.GetMethod("GetColumnInfos");
            //    object obj = Activator.CreateInstance(t);
            //    var columnInfos = method.Invoke(obj, null) as List<ColumnInfo>;
            //    AddColumnInfo(columnInfos);

            //    //补齐列
            //    var method2 = t.GetMethod("SetColumnInfosMoreData");
            //    method2.Invoke(obj, new object[] { ColumnInfos });
            //}


        }
        #endregion

        #endregion

        #region 按钮的相关方法

        #region 1 附加按钮的方法
        /// <summary>
        /// 附加按钮单个
        /// </summary>
        /// <param name="button"></param>
        public virtual void MakeThisButtons(ModuleButton button)
        {
            #region 基础按钮预处理
            if (Buttons == null)
            {
                Buttons = new List<ModuleButton>();
            }
            #endregion

            //判断是不是重复
            if (!HaveButton(button.ButtonKey))
            {
                button.TenantId = TenantId;
                Buttons.Add(button);
            }
        }
        /// <summary>
        /// 附加按钮多个
        /// </summary>
        /// <param name="buttons"></param>
        public virtual void MakeThisButtons(List<ModuleButton> buttons)
        {
            #region 基础按钮预处理
            if (Buttons == null)
            {
                Buttons = new List<ModuleButton>();
            }
            #endregion

            if (Buttons.Count == 0)
            {
                foreach (var button in buttons)
                {
                    button.TenantId = TenantId;
                    Buttons.Add(button);
                }
            }
            else
            {
                foreach (var button in buttons)
                {
                    //判断是不是重复
                    if (!HaveButton(button.ButtonKey))
                    {
                        button.TenantId = TenantId;
                        Buttons.Add(button);
                    }
                }
            }

        }
        #endregion

        #region 2 判断按钮是否重复 return bool
        /// <summary>
        /// 判断是否有了对应按钮
        /// </summary>
        /// <param name="ButtonKey"></param>
        /// <returns></returns>
        public bool HaveButton(string ButtonKey) => Buttons != null && Buttons.Count(o => o.ButtonKey == ButtonKey) > 0;
        //{
        //    bool have = false;
        //    if (Buttons != null && Buttons.Count>0)
        //    {
        //        have = Buttons.Count(o => o.ButtonKey == ButtonKey) > 0;
        //        //if (Buttons.Count > 0)
        //        //{
        //        //    foreach (var Button in Buttons.Count(o=>o.ButtonKey==))
        //        //    {
        //        //        if (Button.ButtonKey == ButtonKey)
        //        //        {
        //        //            have = true;
        //        //            break;
        //        //        }
        //        //    }
        //        //}
        //    }
        //    return have;
        //}
        #endregion

        #region 按钮构建的提供方法 1 构建默认按钮
        /// <summary>
        /// 构建默认按钮
        /// </summary>
        /// <param name="canDel">默认删除按钮</param>
        /// <param name="canAdd">默认添加按钮</param>
        /// <param name="canEdit">默认编辑按钮</param>
        /// <param name="canView">默认查看按钮</param>
        /// <param name="canMultiEdit">默认批量修改按钮</param>
        public virtual void MakeDefaultButtons(bool canDel = true, bool canAdd = true, bool canEdit = true, bool canView = true, bool canMultiEdit = true)
        {
            var pluginName = GetPluginName();

            #region 基础按钮预处理
            if (Buttons == null)
            {
                Buttons = new List<ModuleButton>();
            }
            #endregion

            #region 默认按钮

            var serviceName = IsInterModule ? ModuleKey : "ModuleData";
            var camelServiceName = serviceName.Substring(0, 1).ToLower() + serviceName.Substring(1);

            //默认删除按钮
            var DelButton = new ModuleButton()
            {
                ButtonKey = "Delete",
                ButtonName = "删除",
                ConfirmMsg = "确认删除？",
                ButtonType = ButtonType.ForSingleRow | ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Ajax,
                ButtonActionUrl = $"abp.services.{pluginName}.{camelServiceName}.deleteEntity",
                ButtonClass = "layui-btn-danger",
                Sort = 3
            };

            //默认添加按钮
            var AddButton = new ModuleButton()
            {
                ButtonKey = "Add",
                ButtonName = "添加",
                ButtonType = ButtonType.ForNoneRow,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionParam = "{\"area\": [\"80%\", \"90%\"]}",
                ButtonActionUrl = $"/{serviceName}/Add",
                ButtonClass = "",
                Sort = 1
            };

            //默认编辑按钮
            var EditButton = new ModuleButton()
            {
                ButtonKey = "Edit",
                ButtonName = "编辑",
                ButtonType = ButtonType.ForSingleRow,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionParam = "{\"area\": [\"80%\", \"90%\"]}",
                ButtonActionUrl = $"/{serviceName}/Edit",
                ButtonClass = "",
                Sort = 0
            };

            //默认查看按钮
            var ViewButton = new ModuleButton()
            {
                ButtonKey = "View",
                ButtonName = "查看",
                ButtonType = ButtonType.ForSingleRow,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionParam = "{\"area\": [\"80%\", \"90%\"],\"btn\":null}",
                ButtonActionUrl = $"/{serviceName}/View",
                ButtonClass = "",
                Sort = 0
            };

            //默认批量修改按钮
            var MultiEditButton = new ModuleButton()
            {
                ButtonKey = "MultiEdit",
                ButtonName = "批量修改",
                ButtonType = ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionParam = "{\"area\": [\"80%\", \"90%\"]}",
                ButtonActionUrl = $"/{serviceName}/MultiEdit",
                ButtonClass = "",
                Sort = 5
            };


            #endregion

            #region 构建按钮 通过HaveButton判断是否重复
            //设定删除按钮
            if (canDel && !HaveButton("Delete"))
            {
                DelButton.TenantId = TenantId;
                Buttons.Add(DelButton);
            }

            //设定添加按钮
            if (canAdd && !HaveButton("Add"))
            {
                AddButton.TenantId = TenantId;
                Buttons.Add(AddButton);
            }

            //设定编辑按钮
            if (canEdit && !HaveButton("Edit"))
            {
                EditButton.TenantId = TenantId;
                Buttons.Add(EditButton);
            }

            //modi20181226查看按钮取消
            //设定查看按钮
            //if (canView && !HaveButton("View"))
            //{
            //    ViewButton.TenantId = TenantId;
            //    Buttons.Add(ViewButton);
            //}

            //设定批量修改按钮
            //if (canMultiEdit && !HaveButton("MultiEdit"))
            //{
            //    MultiEditButton.TenantId = TenantId;
            //    Buttons.Add(MultiEditButton);
            //}

            #endregion
        }
        #endregion

        #region 按钮构建的提供方法 2 通过类反射获取自定义的按钮信息,建立按钮信息
        /// <summary>
        /// 构建自定义按钮
        /// </summary>
        /// <param name="id"></param>
        public virtual void CreateCustomizeButtons(string type)
        {
            using (var typeFinderWrapper = IocManager.Instance.ResolveAsDisposable<ITypeFinder>())
            {
                //var types = typeFinderWrapper.Object.Find(o => o.FullName == "Master.EntityFrameworkCore.Seed.BaseData." + type + "BaseModules");
                var types = typeFinderWrapper.Object.Find(o => o.FullName.EndsWith( "." + type + "BaseModules"));
                foreach(var t in types)
                {
                    var method = t.GetMethod("GetModuleButtons");
                    object obj = Activator.CreateInstance(t);
                    var modulebuttons = method.Invoke(obj, null) as List<ModuleButton>;
                    MakeThisButtons(modulebuttons);
                    //补齐按钮
                    var method2 = t.GetMethod("SetButtonsInfosMoreData");
                    method2.Invoke(obj, new object[] { Buttons });
                }
                //if (types.Length > 0)
                //{
                //    var t = types[0];
                //    var method = t.GetMethod("GetModuleButtons");
                //    object obj = Activator.CreateInstance(t);
                //    var modulebuttons = method.Invoke(obj, null) as List<ModuleButton>;
                //    MakeThisButtons(modulebuttons);
                //    //补齐按钮
                //    var method2 = t.GetMethod("SetButtonsInfosMoreData");
                //    method2.Invoke(obj, new object[] { Buttons });
                //}
            }

            //Type t = Type.GetType("Master.EntityFrameworkCore.Seed.BaseData." + type + "BaseModules, Master.EntityFrameworkCore");
            //if (t != null)
            //{
            //    var method = t.GetMethod("GetModuleButtons");
            //    object obj = Activator.CreateInstance(t);
            //    var modulebuttons = method.Invoke(obj, null) as List<ModuleButton>;
            //    MakeThisButtons(modulebuttons);
            //}


        }
        #endregion


        #endregion


        /// <summary>
        /// 构建默认列和默认按钮
        /// </summary>
        public virtual void MakeDefaultColumnsAndButtons()
        {
            #region 默认按钮
            MakeDefaultButtons();
            //if (Buttons == null)
            //{
            //    Buttons = new List<ModuleButton>();
            //}
            //if (Buttons.Count == 0)
            //{
            //    var serviceName = IsInterModule ? ModuleKey : "ModuleData";
            //    var camelServiceName = serviceName.Substring(0, 1).ToLower() + serviceName.Substring(1);
            //    Buttons.Add(new ModuleButton()
            //    {
            //        ButtonKey = "Delete",
            //        ButtonName = "删除",
            //        ConfirmMsg = "确认删除？",
            //        ButtonType = ButtonType.ForSingleRow | ButtonType.ForSelectedRows,
            //        ButtonActionType = ButtonActionType.Ajax,
            //        ButtonActionUrl = $"abp.services.app.{camelServiceName}.deleteEntity",
            //        ButtonClass = "layui-btn-danger",
            //        Sort = 3
            //    });
            //    Buttons.Add(new ModuleButton()
            //    {
            //        ButtonKey = "Add",
            //        ButtonName = "添加",
            //        ButtonType = ButtonType.ForNoneRow,
            //        ButtonActionType = ButtonActionType.Form,
            //        ButtonActionParam = "{\"area\": [\"80%\", \"90%\"]}",
            //        ButtonActionUrl = $"/{serviceName}/Add",
            //        ButtonClass = "",
            //        Sort = 1
            //    });
            //    Buttons.Add(new ModuleButton()
            //    {
            //        ButtonKey = "Edit",
            //        ButtonName = "编辑",
            //        ButtonType = ButtonType.ForSingleRow,
            //        ButtonActionType = ButtonActionType.Form,
            //        ButtonActionParam = "{\"area\": [\"80%\", \"90%\"]}",
            //        ButtonActionUrl = $"/{serviceName}/Edit",
            //        ButtonClass = "",
            //        Sort = 0
            //    });
            //    Buttons.Add(new ModuleButton()
            //    {
            //        ButtonKey = "View",
            //        ButtonName = "查看",
            //        ButtonType = ButtonType.ForSingleRow,
            //        ButtonActionType = ButtonActionType.Form,
            //        ButtonActionParam = "{\"area\": [\"80%\", \"90%\"],\"btn\":null}",
            //        ButtonActionUrl = $"/{serviceName}/View",
            //        ButtonClass = "",
            //        Sort = 0
            //    });
            //    Buttons.Add(new ModuleButton()
            //    {
            //        ButtonKey = "MultiEdit",
            //        ButtonName = "批量修改",
            //        ButtonType = ButtonType.ForSelectedRows,
            //        ButtonActionType = ButtonActionType.Form,
            //        ButtonActionParam = "{\"area\": [\"80%\", \"90%\"]}",
            //        ButtonActionUrl = $"/{serviceName}/MultiEdit",
            //        ButtonClass = "",
            //        Sort = 5
            //    });
            //}
            #endregion

            #region 默认列
            MakeDefaultColumns();
            //if (ColumnInfos == null)
            //{
            //    ColumnInfos = new List<ColumnInfo>();
            //}
            //if (ColumnInfos.Count == 0)
            //{
            //    ColumnInfos.Add(new Model.ColumnInfo()
            //    {
            //        ColumnKey = StaticSystemColumns.Creator,
            //        ColumnName = "创建人",
            //        ValuePath = "CreatorUser.Name",
            //        IsInterColumn = true,
            //        IsSystemColumn = true,
            //        IsShownInAdd = false,
            //        IsShownInEdit = false,
            //        ColumnType = Model.ColumnTypes.Text
            //    });
            //    ColumnInfos.Add(new Model.ColumnInfo()
            //    {                    
            //        ColumnKey = StaticSystemColumns.CreationTime,
            //        ColumnName = "创建时间",
            //        ValuePath = StaticSystemColumns.CreationTime,
            //        IsInterColumn = true,
            //        IsSystemColumn = true,
            //        IsShownInAdd = false,
            //        IsShownInEdit = false,
            //        ColumnType = Model.ColumnTypes.DateTime,
            //        DisplayFormat = "yyyy-MM-dd HH:mm:ss",
            //        ControlFormat = "datetime"
            //    });
            //    ColumnInfos.Add(new Model.ColumnInfo()
            //    {
            //        ColumnKey = StaticSystemColumns.Modifier,
            //        ColumnName = "修改人",
            //        ValuePath = "LastModifierUser.Name",
            //        IsInterColumn = true,
            //        IsSystemColumn = true,
            //        IsShownInAdd = false,
            //        IsShownInEdit = false,
            //        ColumnType = Model.ColumnTypes.Text
            //    });
            //    ColumnInfos.Add(new Model.ColumnInfo()
            //    {
            //        ColumnKey = StaticSystemColumns.ModifyTime,
            //        ColumnName = "修改时间",
            //        ValuePath = StaticSystemColumns.ModifyTime,
            //        IsInterColumn = true,
            //        IsSystemColumn = true,
            //        IsShownInAdd = false,
            //        IsShownInEdit = false,
            //        ColumnType = Model.ColumnTypes.DateTime,
            //        DisplayFormat = "yyyy-MM-dd HH:mm:ss",
            //        ControlFormat = "datetime"
            //    });
            //    var operationColumnInfo = new Model.ColumnInfo()
            //    {
            //        ColumnKey = StaticSystemColumns.Operation,
            //        ColumnName = "操作",
            //        IsInterColumn = true,
            //        IsSystemColumn = true,
            //        ColumnType = Model.ColumnTypes.System,
            //        IsShownInAdd = false,
            //        IsShownInEdit = false,
            //        IsShownInView = false
            //    };
            //    operationColumnInfo.SetData("fixed", "right");
            //    ColumnInfos.Add(operationColumnInfo);
            //}
            #endregion

        }












        /// <summary>
        /// 根据表单类型过滤列
        /// </summary>
        /// <param name="formType"></param>
        /// <returns></returns>
        public virtual IEnumerable<ColumnInfo> FilterdColumnInfos(FormType formType)
        {
            IEnumerable<ColumnInfo> result = null;
            switch (formType)
            {
                case FormType.Add:
                    result = ColumnInfos.Where(o => o.IsShownInAdd && o.CheckAddPermission());
                    break;
                case FormType.Edit:
                    result = ColumnInfos.Where(o => o.IsShownInEdit && o.CheckEditPermission());
                    break;
                case FormType.View:
                    result = ColumnInfos.Where(o => o.IsShownInView && o.CheckViewPermission());
                    break;
                case FormType.MultiEdit:
                    result = ColumnInfos.Where(o => o.IsShownInMultiEdit && !o.IsSystemColumn && o.CheckEditPermission());
                    break;
                case FormType.Search:
                    result = ColumnInfos.Where(o => o.IsShownInAdvanceSearch && o.CheckViewPermission() && (o.IsDirectiveColumn || o.IsPropertyColumn) && (o.ColumnType != ColumnTypes.Files && o.ColumnType != ColumnTypes.Images));
                    break;
                case FormType.List:
                    result = ColumnInfos.Where(o => o.IsShownInList && o.CheckViewPermission());
                    break;

            }
            return result.OrderBy(o => o.Sort);
        }
        #endregion
    }

    public enum SortType
    {
        Asc,
        Desc
    }
}
