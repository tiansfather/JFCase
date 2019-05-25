using Abp;
using Abp.Dependency;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    /// <summary>
    /// 登录失败提示帮助类
    /// </summary>
    public class LoginResultTypeHelper : AbpServiceBase, ITransientDependency
    {
        public LoginResultTypeHelper()
        {
            LocalizationSourceName = MasterConsts.LocalizationSourceName;
        }

        public Exception CreateExceptionForFailedLoginAttempt(LoginResultType result, string username, string tenancyName)
        {
            switch (result)
            {
                case LoginResultType.Success:
                    return new Exception("Don't call this method with a success result!");
                case LoginResultType.InvalidUserName:
                case LoginResultType.InvalidPassword:
                    return new UserFriendlyException("登录失败","您输入的用户名不存在或密码错误，请重新输入");
                case LoginResultType.InvalidTenancyName:
                    return new UserFriendlyException(L("登录失败"), L("账套{0}不存在", tenancyName));
                case LoginResultType.TenantIsNotActive:
                    return new UserFriendlyException(L("登录失败"), L("此账套未启用或处于禁用状态,请联系客服15906863922", tenancyName));
                case LoginResultType.UserIsNotActive:
                    return new UserFriendlyException("登录失败", "此用户已被禁用");
                case LoginResultType.LockedOut:
                    return new UserFriendlyException(L("登录失败"), "由于密码错误次数过多,此用户已被锁定登录");
                case LoginResultType.UnknownExternalLogin:
                    return new UserFriendlyException("未知来源");
                default: // Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException(L("LoginFailed"));
            }
        }
    }
}
