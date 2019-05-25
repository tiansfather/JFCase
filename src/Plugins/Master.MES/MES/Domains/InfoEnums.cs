using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Domains
{
    /// <summary>
    /// 员人数量
    /// </summary>
    public enum EmployeeNumber
    {
        小于10人,
        大于10人小于20人,
        大于20人小于50人,
        大于50人
    }

    /// <summary>
    /// 产值规格
    /// </summary>
    public enum OutputValue
    {
        小于100万,
        大于100万小于500万,
        大于500万小于1000万,
        大于1000万
    }
}
