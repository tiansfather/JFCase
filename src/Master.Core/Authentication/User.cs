using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Entity;
using Master.Module;
using Master.Module.Attributes;
using Master.MultiTenancy;
using Master.Organizations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Authentication
{
    [InterModule("用户管理")]
    public class User:FullAuditedEntity<long>, IMayHaveTenant,  IPassivable, IExtendableObject,IAutoEntity,IMayHaveOrganization,IHaveProperty,IHaveStatus
    {
        public const string Status_NotVerified = "NotVerified";
        public const string AdminUserName= "admin";
        public const string AdminUserPassword = "12345678";
        //public const int LockoutTimeSpan = 60;//秒
        //public const int MaxLockoutFailCount = 6;//6次失败后锁定
        public virtual int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        [InterColumn(ColumnName = "账号",Sort = 0,IsShownInAdd =false,IsShownInEdit =false,IsShownInMultiEdit =false)]
        public virtual string UserName { get; set; }
        [InterColumn(ColumnName = "姓名", VerifyRules = "required", Sort =1)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [MaxLength(2)]
        public virtual string Sex { get; set; }
        public virtual string Password { get; set; }
        public virtual string PhoneNumber { get; set; }
        //[InterColumn(ColumnName = "律师事务所", Sort = 2)]
        public virtual string WorkLocation { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [DataType(DataType.Date)]
        public virtual DateTime? BirthDay { get; set; }
        public virtual string Email { get; set; }
        [InterColumn(ColumnName = "所属组织", ColumnType = ColumnTypes.Text, Renderer = "lay-departchoose", DisplayPath = "Organization.DisplayName", Templet = "{{d.organizationId_display?d.organizationId_display:''}}", Sort = 2)]
        public int? OrganizationId { get; set; }

        [InterColumn(ColumnName ="激活状态",ColumnType =ColumnTypes.Switch,IsShownInAdd =false,IsShownInEdit =false,Templet = "{{d.isActive?'<span class=\"layui-badge layui-bg-green\">激活</span>':'<span class=\"layui-badge layui-bg-gray\">冻结</span>'}}",Sort =3)]
        public virtual bool IsActive { get; set; } = true;
        /// <summary>
        /// 是否需要审核
        /// </summary>
        public virtual bool ToBeVerified { get; set; } = false;
        /// <summary>
        /// Lockout end date.
        /// </summary>
        public virtual DateTime? LockoutEndDate { get; set; }

        /// <summary>
        /// Gets or sets the access failed count.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserPermissionSetting> Permissions { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserLogin> Logins { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> Roles { get; set; }
        public virtual DateTime? LastLoginTime { get; set; }
        public virtual string ExtensionData { get; set; }
        public virtual User CreatorUser { get; set; }
        public virtual User LastModifierUser { get; set; }
        public virtual User DeleterUser { get; set; }
        
        public virtual Organization Organization { get; set; }
        [Column(TypeName ="json")]
        public JsonObject<IDictionary<string, object>> Property { get; set; }

        
        public string Status { get;set; }

        [NotMapped]
        public bool IsStrongPwd
        {
            get
            {
                return this.GetPropertyValue<bool>("IsStrongPwd");
            }
            set
            {
                this.SetPropertyValue("IsStrongPwd", value);
            }
        }
        [NotMapped]
        public bool MustChangePwd
        {
            get
            {
                return this.GetPropertyValue<bool>("MustChangePwd");
            }
            set
            {
                this.SetPropertyValue("MustChangePwd", value);
            }
        }
        [NotMapped]
        public bool IsFirstLogin
        {
            get
            {
                return this.GetPropertyValue<bool>("IsFirstLogin");
            }
            set
            {
                this.SetPropertyValue("IsFirstLogin", value);
            }
        }
        [NotMapped]
        public bool NeedChangePwd
        {
            get
            {
                return IsFirstLogin && MustChangePwd;
            }
        }
        /// <summary>
        /// 生成账套管理员用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static User CreateTenantAdminUser(int tenantId)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Password=Abp.Runtime.Security.SimpleStringCipher.Instance.Encrypt(AdminUserPassword)
            };

            return user;
        }

        /// <summary>
        /// 生成主体管理员用户
        /// </summary>
        /// <returns></returns>
        public static User CreateHostAdminUser()
        {
            var user = new User
            {
                TenantId = null,
                UserName = AdminUserName,
                Name = AdminUserName,
                Password = Abp.Runtime.Security.SimpleStringCipher.Instance.Encrypt(AdminUserPassword)
            };

            return user;
        }

        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TenantId, Id);
        }

    }
    public class UserEntityMapConfiguration : EntityMappingConfiguration<User>
    {
        public override void Map(EntityTypeBuilder<User> b)
        {
            b.HasOne(p => p.DeleterUser)
                .WithMany()
                .HasForeignKey(p => p.DeleterUserId);

            b.HasOne(p => p.CreatorUser)
                .WithMany()
                .HasForeignKey(p => p.CreatorUserId);

            b.HasOne(p => p.LastModifierUser)
                .WithMany()
                .HasForeignKey(p => p.LastModifierUserId);
            b.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId);

            b.HasOne(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId);
        }
    }
}
