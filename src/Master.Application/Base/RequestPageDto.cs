using Master.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Dto
{
    public class RequestPageDto:RequestDto
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// 每页数量
        /// </summary>
        public int Limit { get; set; } = 20;
        /// <summary>
        /// 查询条件 Name="a" or CreatorUser.Name.Contains("b")
        /// </summary>
        public string Where { get; set; }
        /// <summary>
        /// 前台的过滤字段集合数据[{columnName:'ProcessSN',conditions:[{andOr:'',leftBracket:'',rightBracket:'',comparer:'=',value:'201810120011'}]}]
        /// </summary>
        public string TableFilter { get; set; }
        /// <summary>
        /// 过滤字段,用于列筛选
        /// </summary>
        public string FilterField { get; set; }
        /// <summary>
        /// 过滤字段查询关键字,用于列筛选
        /// </summary>
        public string FilterKey { get; set; }
        /// <summary>
        /// 需要返回筛选数据的列["unitId","projectType","orderDate","customerProjectSN","customerName","projectPic","projectSN","projectName","projectPart","requireDate","t0Date","projectTechnology","number","projectCharger","projectWeight","projectTracker","productDesign","mouldDesign","salesman","lastModificationTime","lastModifierUserId","creationTime","creatorUserId"]
        /// </summary>
        public string FilterColumns { get; set; }
        /// <summary>
        /// 通用高级检索
        /// </summary>
        public string SearchCondition { get; set; }
        /// <summary>
        /// SoulTable高级检索
        /// </summary>
        public string FilterSos { get; set; }
        /// <summary>
        /// 页面内置检索
        /// </summary>
        public string SearchKeys { get; set; }
        /// <summary>
        /// 关键字检索
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderField { get; set; } = "Id";
        /// <summary>
        /// 排序方式:asc,desc
        /// </summary>
        public string OrderType { get; set; } = "desc";
    }
}
