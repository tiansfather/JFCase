using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    [AutoMap(typeof(ColumnInfo))]
    public class ColumnInfoDto
    {
        public int Id { get; set; }
        public int ModuleInfoId { get; set; }
        public int TenantId { get; set; }
        public virtual string ExtensionData { get; set; }
        public virtual bool EnableFieldPermission { get; set; }
        public virtual ColumnTypes ColumnType { get; set; }
        public virtual string ControlFormat { get; set; }

        public virtual int Sort { get; set; }
        public virtual string ColumnKey { get; set; }
        public virtual string ColumnName { get; set; }
        public virtual string Renderer { get; set; }
        public virtual string Templet { get; set; }
        public virtual int MaxFileNumber { get; set; } 
        public virtual bool IsInterColumn { get; set; }
        public virtual bool IsSystemColumn { get; set; } 
        public virtual string DisplayFormat { get; set; }
        public virtual string DefaultValue { get; set; }
        public virtual string VerifyRules { get; set; }
        public virtual string ValuePath { get; set; }
        public virtual string DisplayPath { get; set; }
        public virtual string DictionaryName { get; set; }
        public virtual string CustomizeControl { get; set; }
        public virtual string ControlParameter { get; set; }
        public virtual bool IsShownInList { get; set; } 
        public virtual bool IsShownInAdd { get; set; } 
        public virtual bool IsShownInEdit { get; set; } 
        public virtual bool IsShownInMultiEdit { get; set; } 
        public virtual bool IsShownInAdvanceSearch { get; set; } 
        public virtual bool IsShownInView { get; set; } 
        public virtual bool IsEnableSort { get; set; }
        public virtual RelativeDataType RelativeDataType { get; set; }
        public virtual string RelativeDataString { get; set; }
        public virtual bool IsPropertyColumn { get; set; }
        public virtual bool IsRelativeColumn { get; set; }
        public virtual bool IsDirectiveColumn { get; set; }
        public virtual bool IsOperationColumn { get; set; }
    }
}
