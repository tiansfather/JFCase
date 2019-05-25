using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Master
{
    [DependsOn(
        typeof(MasterCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class MasterApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MasterApplicationModule).GetAssembly());
        }
    }
}