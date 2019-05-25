using Abp.Authorization;
using Abp.AutoMapper;
using Master.Authentication;
using Master.Domain;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Users
{
    public class NewMinerAppService : ModuleDataAppServiceBase<NewMiner, int>
    {
        protected override string ModuleKey()
        {
            return nameof(NewMiner);
        }

        public virtual async Task Register(NewMinerRegisterDto newMinerRegisterDto)
        {
            var newMiner = newMinerRegisterDto.MapTo<NewMiner>();
            await Manager.InsertAsync(newMiner);
        }
        [AbpAuthorize]
        public virtual async Task Verify(IEnumerable<int> ids)
        {
            var manager = Manager as NewMinerManager;
            var userManger = Resolve<UserManager>();
            var roleManager = Resolve<RoleManager>();

            var role = await roleManager.FindByNameAsync(StaticRoleNames.Tenants.Miner);

            var newMiners = await manager.GetListByIdsAsync(ids);
            foreach(var newMiner in newMiners)
            {
                var user = newMiner.MapTo<User>();
                user.Id = 0;
                user.SetPropertyValue("NickName", newMiner.NickName);
                user.SetPropertyValue("Avata", newMiner.Avata);
                await userManger.InsertAndGetIdAsync(user);
                await userManger.SetRoles(user, new int[] { role.Id });
                user.Logins = new List<UserLogin>() {
                new UserLogin(AbpSession.TenantId,user.Id,"Wechat",newMiner.OpenId)
            };
                newMiner.Verified = true;
                await manager.DeleteAsync(newMiner);
            }
            
        }
    }
}
