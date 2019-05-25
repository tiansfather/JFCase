using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Entities;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class UnitBaseModules : BaseSystemModules
    {
        public override List<ColumnInfo> GetColumnInfos()
        {
            return new List<ColumnInfo>();
        }

        public override List<ModuleButton> GetModuleButtons()
        {
            var buttons = new List<ModuleButton>();
            buttons.Add(new ModuleButton()
            {
                ButtonKey = "Notice",
                ButtonName = "群发公告",
                ButtonClass="layui-btn-normal",
                ButtonActionType = ButtonActionType.Form,
                ButtonType = ButtonType.ForSelectedRows,
                IsEnabled = true,
                ButtonActionUrl = "/Home/Show?name=../MES/TenantNotice",
                ButtonActionParam = "",
                Sort = 6
            });
            return buttons;
        }

        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            
        }

        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            //修改单位名称列的数据
            var column = ColumnInfos.Single(o => o.ColumnKey == "UnitName");
            column.Templet= "{{d.unitName}}{{#if (d.tenantId){}}<span class=\"layui-badge layui-bg-green\" modulekey=\"Unit\" tips=\"{{d.tenancyName}},双击移除绑定\" dataid=\"{{d.id}}\" ondblclick=\"func.callModuleButtonEvent()\" buttonactiontype=\"Ajax\" confirmmsg=\"确认取消绑定?\" buttonactionurl=\"abp.services.app.mESUnit.unBindTenant\">已加入</span>{{#}else{}}<a dataid=\"1\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"邀请加入\" params=\"{&quot;area&quot;: [&quot;500px&quot;, &quot;500px&quot;],&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/Home/Show?name=../MES/Invite&companyName={{d.unitName}}&inviter={{d.inviter}}&unitId={{d.id}}&unitNature={{d.unitNature}}\" onclick=\"func.callModuleButtonEvent()\">邀请加入</a>{{#}}}";
            column.SetData("width", 250);
        }
    }
}
