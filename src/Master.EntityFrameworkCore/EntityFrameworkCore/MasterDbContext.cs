using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Master.Application.Editions;
using Master.Application.Features;
using Master.Auditing;
using Master.Authentication;
using Master.Case;
using Master.Configuration;
using Master.Configuration.Dictionaries;
using Master.Entity;
using Master.EntityFrameworkCore.Repositories;
using Master.Module;
using Master.MultiTenancy;
using Master.Notices;
using Master.Organizations;
using Master.Resources;
using Master.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Master.EntityFrameworkCore
{
    [AutoRepositoryTypes(
    typeof(IRepository<>),
    typeof(IRepository<,>),
    typeof(MasterRepositoryBase<>),
    typeof(MasterRepositoryBase<,>)
    )]
    public class MasterDbContext : AbpDbContext
    {
        #region Entities
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<Edition> Edition { get; set; }
        public virtual DbSet<FeatureSetting> FeatureSetting { get; set; }
        public virtual DbSet<EditionFeatureSetting> EditionFeatureSetting { get; set; }
        public virtual DbSet<TenantFeatureSetting> TenantFeatureSetting { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<UserLoginAttempt> UserLoginAttempt { get; set; }
        public virtual DbSet<PermissionSetting> Permission { get; set; }
        public virtual DbSet<RolePermissionSetting> RolePermission { get; set; }
        public virtual DbSet<UserPermissionSetting> UserPermission { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<ModuleInfo> ModuleInfo { get; set; }
        public virtual DbSet<ModuleData> ModuleData { get; set; }
        public virtual DbSet<ModuleButton> ModuleButton { get; set; }
        public virtual DbSet<ColumnInfo> ColumnInfo { get; set; }
        public DbSet<Dictionary> Dictionary { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<BaseTree> BaseTree { get; set; }
        public virtual DbSet<BaseType> BaseType { get; set; }
        public virtual DbSet<Notice> Notice { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<Resource> Resource { get; set; }
        public virtual DbSet<NewMiner> NewMiner { get; set; }
        public virtual DbSet<CaseSource> CaseSource { get; set; }
        public virtual DbSet<CaseInitial> CaseInitial { get; set; }
        public virtual DbSet<EmailLog> EmailLog { get; set; }
        public virtual DbSet<CaseSourceHistory> CaseSourceHistory { get; set; }
        public virtual DbSet<CaseFine> CaseFine { get; set; }
        public virtual DbSet<CaseNode> CaseNode { get; set; }
        public virtual DbSet<CaseCard> CaseCard { get; set; }
        public virtual DbSet<Label> Label { get; set; }
        public virtual DbSet<TreeLabel> TreeLabel { get; set; }
        #endregion

        public MasterDbContext(DbContextOptions<MasterDbContext> options) 
            : base(options)
        {

        }

        #region DbFunction
        [DbFunction(FunctionName = "json_extract")]
        public static string GetJsonValueString(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
        {
            return "";
        }
        [DbFunction(FunctionName = "json_extract")]
        public static decimal? GetJsonValueNumber(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
        {
            return 0;
        }
        [DbFunction(FunctionName = "json_extract")]
        public static DateTime GetJsonValueDate(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
        {
            return DateTime.Now;
        }
        [DbFunction(FunctionName = "json_extract")]
        public static bool GetJsonValueBool(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
        {
            return true;
        }
        #endregion

        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsSoftDeleteFilterEnabled || !((ISoftDelete) e).IsDeleted
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */

                //Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted || ((ISoftDelete)e).IsDeleted != IsSoftDeleteFilterEnabled;
                Expression<Func<TEntity, bool>> softDeleteFilter = e => !IsSoftDeleteFilterEnabled || !((ISoftDelete)e).IsDeleted;
                expression = expression == null ? softDeleteFilter : CombineExpressions(expression, softDeleteFilter);
            }

            if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */
                //Expression<Func<TEntity, bool>> mayHaveTenantFilter = e => ((IMayHaveTenant)e).TenantId == CurrentTenantId || (((IMayHaveTenant)e).TenantId == CurrentTenantId) == IsMayHaveTenantFilterEnabled;
                Expression<Func<TEntity, bool>> mayHaveTenantFilter = e => !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId;
                expression = expression == null ? mayHaveTenantFilter : CombineExpressions(expression, mayHaveTenantFilter);
            }

            if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsMustHaveTenantFilterEnabled || ((IMustHaveTenant)e).TenantId == CurrentTenantId
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */
                //Expression<Func<TEntity, bool>> mustHaveTenantFilter = e => ((IMustHaveTenant)e).TenantId == CurrentTenantId || (((IMustHaveTenant)e).TenantId == CurrentTenantId) == IsMustHaveTenantFilterEnabled;
                Expression<Func<TEntity, bool>> mustHaveTenantFilter = e => !IsMustHaveTenantFilterEnabled || ((IMustHaveTenant)e).TenantId == CurrentTenantId;
                expression = expression == null ? mustHaveTenantFilter : CombineExpressions(expression, mustHaveTenantFilter);
            }

            return expression;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ///动态加入实体
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes().Where(o => typeof(IAutoEntity).IsAssignableFrom(o) && o.IsClass && !o.IsAbstract))
                {
                    modelBuilder.Model.GetOrAddEntityType(type);
                }
                //通过反射加入实体配置
                modelBuilder.AddEntityConfigurationsFromAssembly(asm);
            }

            base.OnModelCreating(modelBuilder);

            
        }
    }
}
