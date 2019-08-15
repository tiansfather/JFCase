
using Abp.Domain.Entities;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class UserBaseModules:BaseSystemModules
    {
        /// <summary>
        /// 得到对应的基础附加列
        /// </summary>
        /// <returns></returns>
        public override List<ColumnInfo> GetColumnInfos()
        {
            List<ColumnInfo> columnInfos = new List<ColumnInfo>();

            //增加员工部门列
            //var departColumnInfo = new ColumnInfo()
            //{
            //    ColumnKey = "StaffOrganizationUnit",
            //    ColumnName = "部门",
            //    ColumnType = ColumnTypes.Customize,
            //    IsInterColumn = true,
            //    CustomizeControl = "IStaffDepartControl",
            //    ControlParameter = "{\"selectedMulti\":true}",
            //    Templet = "{{d.StaffOrganizationUnit_data}}"
            //};
            //columnInfos.Add(departColumnInfo);



            //增加角色列
            columnInfos.Add(new ColumnInfo()
            {
                ColumnKey = "RoleNames",
                ColumnName = "角色",
                ColumnType = ColumnTypes.Reference,
                RelativeDataType=RelativeDataType.Default,
                IsInterColumn = true,
                IsShownInAdd=false,
                IsShownInEdit=false,
                IsShownInAdvanceSearch=false,
                IsShownInMultiEdit=false
            });
            return columnInfos;
        }
        /// <summary>
        /// 补齐列的属性
        /// </summary>
        /// <returns></returns>
        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            //设置操作列宽度
            var column_operation = ColumnInfos.SingleOrDefault(o => o.ColumnKey == "Operation");
            if (column_operation != null)
            {
                column_operation.SetData("width", "240");
            }

            ColumnInfos.Single(o => o.ColumnKey == "Name").SetData("width", "80");
            ColumnInfos.Single(o => o.ColumnKey == "CreatorUserId").SetData("width", "80");
            ColumnInfos.Single(o => o.ColumnKey == "LastModifierUserId").SetData("width", "80");
        }
        /// <summary>
        /// 得到对应的基础附加按钮
        /// </summary>
        /// <returns></returns>
        public override List<ModuleButton> GetModuleButtons()
        {
            List<ModuleButton> moduleButtons = new List<ModuleButton>();
            ////增加通过按钮
            //var VerifyModuleButton = new ModuleButton()
            //{
            //    ButtonKey = "Verify",
            //    ButtonName = "通过",
            //    ButtonType = ButtonType.ForSelectedRows,
            //    ButtonActionType = ButtonActionType.Ajax,
            //    ButtonActionUrl = "abp.services.app.user.verifyUser",
            //    RequirePermission=false,
            //    ButtonClass = "",
            //    Sort = 6
            //};
            //moduleButtons.Add(VerifyModuleButton);
            //增加权限按钮(有账号的显示)
            var PermissionModuleButton = new ModuleButton()
            {
                ButtonKey = "Permission",
                ButtonName = "权限",
                ButtonType = ButtonType.ForSingleRow ,
                ButtonActionParam= "{\"btn\": []}",
                ButtonActionType = ButtonActionType.Form,
                ButtonActionUrl = $"/Permission/Assign",
                ButtonClass = "",
                Sort = 4
            };
            moduleButtons.Add(PermissionModuleButton);

            //增加账号按钮
            var AccountNumberModuleButton = new ModuleButton()
            {
                ButtonKey = "Account",
                ButtonName = "账号",
                ButtonType = ButtonType.ForSingleRow ,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionUrl = $"/User/Account",
                ButtonActionParam= "{\"area\": [\"100%\", \"100%\"]}",
                ButtonClass = "",
                Sort = 3
            };
            moduleButtons.Add(AccountNumberModuleButton);

            
            return moduleButtons;
        }

        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            
        }
    }
}
