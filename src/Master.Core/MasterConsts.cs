namespace Master
{
    public class MasterConsts
    {
        public const string LocalizationSourceName = "Master";

        public const string ConnectionStringName = "Default";
    }
    /// <summary>
    /// 内置系统列名称
    /// </summary>
    public class StaticSystemColumns
    {
        public const string Creator = "CreatorUserId";
        public const string CreationTime = "CreationTime";
        public const string Modifier = "LastModifierUserId";
        public const string ModifyTime = "LastModificationTime";
        public const string Operation = "Operation";
    }
    /// <summary>
    /// 内置字典名称
    /// </summary>
    public class StaticDictionaryNames
    {
        public const string Sex = "性别";
        public const string Degree = "学历";
        public const string Marriage = "婚姻";
        public const string UnitNature = "单位性质";
        public const string ProjectType = "项目类别";
        public const string SupplierType = "供应类别";
    }
}