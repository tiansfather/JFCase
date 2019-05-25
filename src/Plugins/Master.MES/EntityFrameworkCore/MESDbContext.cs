using Abp.EntityFrameworkCore;
using Master.Authentication;
using Master.MES;
using Master.MultiTenancy;
using Master.Projects;
using Master.Units;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.EntityFrameworkCore
{
    public class MESDbContext : AbpDbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Part> Part { get; set; }
        public DbSet<ProcessType> ProcessType { get; set; }
        public DbSet<ProcessTask> ProcessTask { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<ProcessTaskReport> ProcessTaskReport { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Tactic> Tactic { get; set; }
        public DbSet<TacticPerson> TacticPerson { get; set; }
        public DbSet<RemindLog> RemindLog { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentProcessType> EquipmentProcessType { get; set; }
        public DbSet<EquipmentOperatorHistory> EquipmentOperatorHistory { get; set; }
        public DbSet<ProcessQuote> ProcessQuote { get; set; }
        public DbSet<ProcessQuoteTask> ProcessQuoteTask { get; set; }
        public DbSet<ProcessQuoteBid> ProcessQuoteBid { get; set; }
        public MESDbContext(DbContextOptions<MESDbContext> options)
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
        public static decimal GetJsonValueNumber(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {                
                //通过反射加入实体配置
                modelBuilder.AddEntityConfigurationsFromAssembly(asm);
            }

            base.OnModelCreating(modelBuilder);


        }


    }
}
