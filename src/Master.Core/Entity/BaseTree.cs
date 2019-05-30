using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Extensions;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Master.Entity
{
    /// <summary>
    /// 分类树基类
    /// </summary>
    public class BaseTree : BaseFullEntity, IMayHaveTenant, IHaveSort
    {
        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        /// <summary>
        /// Maximum depth of an UO hierarchy.
        /// </summary>
        public const int MaxDepth = 5;
        public string Discriminator { get; set; }
        /// <summary>
        /// Length of a code unit between dots.
        /// </summary>
        public const int CodeUnitLength = 5;
        [ForeignKey("ParentId")]
        public virtual BaseTree Parent { get; set; }
        public virtual int? ParentId { get; set; }
        /// <summary>
        /// 树层级编码
        /// </summary>
        public virtual string Code { get; set; }

        [Required]
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string BriefCode { get; set; }
        /// <summary>
        /// Children of this OU.
        /// </summary>
        public virtual ICollection<BaseTree> Children { get; set; }
        public int Sort { get; set; }


        public TreeNodeType TreeNodeType { get; set; }
        /// <summary>
        /// 启用多选
        /// </summary>

        public bool EnableMultiSelect { get; set; }
        /// <summary>
        /// 关联树节点
        /// </summary>
        public int? RelativeNodeId { get; set; }
        

        #region 方法


        public BaseTree()
        {

        }

        public BaseTree(string displayName, int? parentId = null)
        {
            DisplayName = displayName;
            ParentId = parentId;
        }

        /// <summary>
        /// Creates code for given numbers.
        /// Example: if numbers are 4,2 then returns "00004.00002";
        /// </summary>
        /// <param name="numbers">Numbers</param>
        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers.Select(number => number.ToString(new string('0', CodeUnitLength))).JoinAsString(".");
        }

        /// <summary>
        /// Appends a child code to a parent code. 
        /// Example: if parentCode = "00001", childCode = "00042" then returns "00001.00042".
        /// </summary>
        /// <param name="parentCode">Parent code. Can be null or empty if parent is a root.</param>
        /// <param name="childCode">Child code.</param>
        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        /// <summary>
        /// Gets relative code to the parent.
        /// Example: if code = "00019.00055.00001" and parentCode = "00019" then returns "00055.00001".
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="parentCode">The parent code.</param>
        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }

        /// <summary>
        /// Calculates next code for given code.
        /// Example: if code = "00019.00055.00001" returns "00019.00055.00002".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        /// <summary>
        /// Gets the last unit code.
        /// Example: if code = "00019.00055.00001" returns "00001".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        /// <summary>
        /// Gets parent code.
        /// Example: if code = "00019.00055.00001" returns "00019.00055".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }

        #endregion
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum TreeNodeType
    {
        /// <summary>
        /// 知识树节点
        /// </summary>
        Knowledge=0,
        /// <summary>
        /// 分类节点
        /// </summary>
        Type=1
    }

    //public class BaseTreeEntityMapConfiguration : EntityMappingConfiguration<BaseTree>
    //{
        
    //    public override void Map(EntityTypeBuilder<BaseTree> b)
    //    {
    //        //b.HasOne(p => p.Parent)
    //        //    .WithMany()
    //        //    .HasForeignKey(p => p.ParentId);

    //        b.HasOne(p => p.RelativeNode)
    //            .WithMany()
    //            .HasForeignKey(p => p.RelativeNodeId);
    //    }
    //}
}
