using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    [AutoMap(typeof(ModuleButton))]
    public class BtnInfoDto: IShouldNormalize
    {
        public int Id { get; set; }
        public int ModuleInfoId { get; set; }
        public int TenantId { get; set; }
        public virtual string ButtonKey { get; set; }
        public virtual string ButtonName { get; set; }
        public virtual string ButtonClass { get; set; }
        public virtual ButtonActionType ButtonActionType { get; set; } 
        public virtual ButtonType ButtonType { get; set; }
        public virtual string ButtonActionUrl { get; set; }
        public virtual string ButtonActionParam { get; set; }
        public virtual string ConfirmMsg { get; set; }
        public virtual string ButtonScript { get; set; }
        public int Sort { get; set; }
        public virtual bool IsEnabled { get; set; } = true;
        public virtual bool RequirePermission { get; set; } 
        public virtual string ClientShowCondition { get; set; }
        public virtual string TitleTemplet { get; set; }
        public virtual bool IsForSingleRow { get; set; }
        public virtual bool IsForNoneRow { get; set; }
        public virtual bool IsForSelectedRows { get; set; }

        public virtual void Normalize()
        {
            //初始化buttontype
            int buttonTypeValue = 0;
            if (IsForNoneRow)
            {
                buttonTypeValue += Convert.ToInt32(ButtonType.ForNoneRow);
            }
            if (IsForSelectedRows)
            {
                buttonTypeValue+= Convert.ToInt32(ButtonType.ForSelectedRows);
            }
            if (IsForSingleRow)
            {
                buttonTypeValue += Convert.ToInt32(ButtonType.ForSingleRow);
            }

            ButtonType = (ButtonType)buttonTypeValue;
        }
    }
}
