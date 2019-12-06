using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Web.Authorization;
using Abp.Web.Features;
using Abp.Web.Localization;
using Abp.Web.MultiTenancy;
using Abp.Web.Navigation;
using Abp.Web.Sessions;
using Abp.Web.Settings;
using Abp.Web.Timing;
using Castle.Core.Logging;
using Master.Authentication;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Startup
{
    public class MyAuthorizationScriptManager : IAuthorizationScriptManager, ITransientDependency
    {
        /// <inheritdoc/>
        public IAbpSession AbpSession { get; set; }
        public ILogger Logger { get; set; }

        private readonly IPermissionManager _permissionManager;

        public IPermissionChecker PermissionChecker { get; set; }

        /// <inheritdoc/>
        public MyAuthorizationScriptManager(IPermissionManager permissionManager)
        {
            AbpSession = NullAbpSession.Instance;
            PermissionChecker = NullPermissionChecker.Instance;

            _permissionManager = permissionManager;
        }

        /// <inheritdoc/>
        public async Task<string> GetScriptAsync()
        {
            Logger.Info("StartGetScript1");
            var allPermissionNames = _permissionManager.GetAllPermissions(false).Select(p => p.Name).ToList();
            Logger.Info("StartGetScript2");
            var grantedPermissionNames = new List<string>();

            if (AbpSession.UserId.HasValue)
            {
                using (var userManagerWrapper = IocManager.Instance.ResolveAsDisposable<UserManager>())
                {
                    var grantedPermissions = await userManagerWrapper.Object.GetGrantedPermissionsAsync(AbpSession.UserId.Value);
                    grantedPermissionNames = grantedPermissions.Select(o => o.Name).ToList();
                }
                //foreach (var permissionName in allPermissionNames)
                //{
                //    if (await PermissionChecker.IsGrantedAsync(permissionName))
                //    {
                //        grantedPermissionNames.Add(permissionName);
                //    }
                //}
            }
            Logger.Info("StartGetScript3");
            var script = new StringBuilder();

            script.AppendLine("(function(){");

            script.AppendLine();

            script.AppendLine("    abp.auth = abp.auth || {};");

            script.AppendLine();

            AppendPermissionList(script, "allPermissions", allPermissionNames);

            script.AppendLine();

            AppendPermissionList(script, "grantedPermissions", grantedPermissionNames);

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }

        private static void AppendPermissionList(StringBuilder script, string name, IReadOnlyList<string> permissions)
        {
            script.AppendLine("    abp.auth." + name + " = {");

            for (var i = 0; i < permissions.Count; i++)
            {
                var permission = permissions[i];
                if (i < permissions.Count - 1)
                {
                    script.AppendLine("        '" + permission + "': true,");
                }
                else
                {
                    script.AppendLine("        '" + permission + "': true");
                }
            }

            script.AppendLine("    };");
        }
    }
}
