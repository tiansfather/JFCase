using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Entities;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class ProjectBaseModules : BaseSystemModules
    {
        public override List<ColumnInfo> GetColumnInfos()
        {
            //增加模具组长、项目负责人信息列
            var columnInfos = new List<ColumnInfo>();
            var projectChargerColumnInfo = new ColumnInfo()
            {
                ColumnKey = "ProjectCharger",
                ColumnName = "模具组长",
                ColumnType = ColumnTypes.Text,
                IsInterColumn = true,
                ValuePath="Property",
                Sort = 7,
            };
            columnInfos.Add(projectChargerColumnInfo);
            var projectTrackerColumnInfo = new ColumnInfo()
            {
                ColumnKey = "ProjectTracker",
                ColumnName = "项目负责",
                ColumnType = ColumnTypes.Text,
                IsInterColumn = true,
                ValuePath = "Property",
                Sort = 8,
            };
            columnInfos.Add(projectTrackerColumnInfo);
            var productDesignColumnInfo = new ColumnInfo()
            {
                ColumnKey = "ProductDesign",
                ColumnName = "产品设计",
                ColumnType = ColumnTypes.Text,
                IsInterColumn = true,
                ValuePath = "Property",
                Sort = 9,
            };
            columnInfos.Add(productDesignColumnInfo);

            var mouldDesignColumnInfo = new ColumnInfo()
            {
                ColumnKey = "MouldDesign",
                ColumnName = "模具设计",
                ColumnType = ColumnTypes.Text,
                IsInterColumn = true,
                ValuePath = "Property",
                Sort = 10,
            };
            columnInfos.Add(mouldDesignColumnInfo);

            var salesmanColumnInfo = new ColumnInfo()
            {
                ColumnKey = "Salesman",
                ColumnName = "业务员",
                ColumnType = ColumnTypes.Text,
                IsInterColumn = true,
                ValuePath = "Property",
                Sort = 11,
            };
            columnInfos.Add(salesmanColumnInfo);

            var t0ColumnInfo = new ColumnInfo()
            {
                ColumnKey="T0Date",
                ColumnName="T0日期",
                ColumnType=ColumnTypes.DateTime,
                IsInterColumn=true,
                ValuePath="Property",
                Sort=6
            };
            columnInfos.Add(t0ColumnInfo);
            return columnInfos;
        }

        public override List<ModuleButton> GetModuleButtons()
        {
            var buttons= new List<ModuleButton>();
            buttons.Add(new ModuleButton()
            {
                ButtonKey="Schedule",
                ButtonName="进度",
                ButtonActionType=ButtonActionType.Form,
                ButtonType=ButtonType.ForSingleRow,
                IsEnabled=true,
                ButtonActionUrl="/Scheduler/ProjectScheduler",
                ButtonActionParam= "{\"area\": [\"100%\", \"100%\"],\"btn\":[]}"
            });
            buttons.Add(new ModuleButton()
            {
                ButtonKey="Import",
                ButtonName="导入",
                ButtonActionType=ButtonActionType.Form,
                ButtonType=ButtonType.ForNoneRow,
                IsEnabled=true,
                ButtonActionUrl= "/Import/?type=Master.MES.Dtos.MESProjectImportDto",
                ButtonActionParam = "{\"area\": [\"100%\", \"100%\"],\"btn\":[]}",
                Sort=5
            });
            buttons.Add(new ModuleButton()
            {
                ButtonKey="ShowAll",
                ButtonName="查看所有",
                ButtonActionType=ButtonActionType.Resource,
                IsEnabled=true,
                RequirePermission=false,
                Sort=6
            });
            return buttons;
        }

        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            
        }

        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            var projectSNColumn = ColumnInfos.SingleOrDefault(o => o.ColumnKey == "ProjectSN");
            if (projectSNColumn != null)
            {
                projectSNColumn.ColumnName = "模具编号";
            }            
            var projectNameColumn = ColumnInfos.SingleOrDefault(o => o.ColumnKey == "ProjectName");
            if (projectNameColumn != null)
            {
                projectNameColumn.ColumnName = "模具名称";
            }            
            var projectPicColumn = ColumnInfos.SingleOrDefault(o => o.ColumnKey == "ProjectPic");
            if (projectPicColumn != null)
            {
                projectPicColumn.ColumnName = "产品图片";
            }
            var customerProjectSNColumn = ColumnInfos.SingleOrDefault(o => o.ColumnKey == "CustomerProjectSN");
            if (customerProjectSNColumn != null)
            {
                customerProjectSNColumn.ColumnName= "客户模具编号";
            }
            var projectTypeColumn= ColumnInfos.SingleOrDefault(o => o.ColumnKey == "ProjectType");
            if (projectTypeColumn != null)
            {
                projectTypeColumn.ColumnName = "模具类别";
            }
        }
    }
}
