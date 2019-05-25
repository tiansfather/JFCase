using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    /// <summary>
    /// 静态角色
    /// </summary>
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        public static class Tenants
        {
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string Charger = "Charger";
            public const string Miner = "Miner";
            public const string Assistant = "Assistant";
        }
    }
}
