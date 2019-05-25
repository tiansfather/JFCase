using Master.Entity;
using Master.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES
{
    public static class UnitExtension
    {
        /// <summary>
        /// 往来单位是否已绑定账套
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool IsTenantBinded(this Unit unit)
        {
            return GetTenantId(unit) != null;
        }
        /// <summary>
        /// 获取往来单位绑定的账套Id
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static int? GetTenantId(this Unit unit)
        {
            return unit.GetPropertyValue<int?>("TenantId");
        }
    }
}
