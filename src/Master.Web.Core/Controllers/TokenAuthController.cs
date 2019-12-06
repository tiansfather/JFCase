using Master.Authentication.JwtBearer;
using Master.Models.TokenAuth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Master.MultiTenancy;
using Master.Authentication;
using Master.Authentication.External;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Abp.Domain.Entities;

namespace Master.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : MasterControllerBase
    {
        private readonly ITenantCache _tenantCache;
        private readonly LoginResultTypeHelper _loginResultTypeHelper;
        private readonly LoginManager _loginManager;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;

        public IHttpContextAccessor httpContextAccessor { get; set; }
        public TenantManager TenantManager { get; set; }
        public UserManager UserManager { get; set; }
        public TokenAuthController(
            TokenAuthConfiguration configuration,
            LoginManager loginManager,
            LoginResultTypeHelper loginResultTypeHelper,
            ITenantCache tenantCache,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager
            )
        {
            _configuration = configuration;
            _loginManager = loginManager;
            _loginResultTypeHelper = loginResultTypeHelper;
            _tenantCache = tenantCache;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
        }

        

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<AuthenticateResultModel> Authenticate(AuthenticateModel model)
        {
            //先判断验证码
            if (HttpContext.Session.Get<string>("CaptchaCode")?.ToLower() != model.VerifyCode.ToLower())
            {
                throw new UserFriendlyException("您输入的验证码不正确，如看不清，可点击验证码更换");
            }
            var loginResult = await GetLoginResultAsync(
                model.UserName,
                model.Password,
                model.TenancyName
            );

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
            //var permissions = await UserManager.GetGrantedPermissionsAsync(loginResult.User);
            
            var result= new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id,
                //GrantedPermissions=permissions.Select(o=>o.Name)
            };

            loginResult.User.SetData("currentToken", result.EncryptedAccessToken);

            await UserManager.UpdateAsync(loginResult.User);

            HttpContext.Session.Set("LoginInfo", loginResult.User.Id);

            return result;
        }
        /// <summary>
        /// 所有第三方登录提供者
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }
        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate(ExternalAuthenticateModel model)
        {
            //var externalUser = await GetExternalUserInfo(model);

            var loginResult = await _loginManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider));
            switch (loginResult.Result)
            {
                case LoginResultType.Success:
                    {
                        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                        var permissions = await UserManager.GetGrantedPermissionsAsync(loginResult.User);
                        var result= new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                            GrantedPermissions = permissions.Select(o => o.Name)
                        };
                        //如果是电脑浏览器登录，则需要记录当前token,用于限制同一时间单账号登录
                        if (model.ClientInfo == "Browser")
                        {
                            loginResult.User.SetData("currentToken", result.EncryptedAccessToken);
                        }
                        
                        await UserManager.UpdateAsync(loginResult.User);

                        HttpContext.Session.Set("LoginInfo", loginResult.User.Id);

                        return result;
                    }
                //case LoginResultType.UnknownExternalLogin:
                //    {
                //        var newUser = await RegisterExternalUserAsync(externalUser);
                //        if (!newUser.IsActive)
                //        {
                //            return new ExternalAuthenticateResultModel
                //            {
                //                WaitingForActivation = true
                //            };
                //        }

                //        //Try to login again with newly registered user!
                //        loginResult = await _loginManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
                //        if (loginResult.Result != LoginResultType.Success)
                //        {
                //            throw _loginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                //                loginResult.Result,
                //                model.ProviderKey,
                //                GetTenancyNameOrNull()
                //            );
                //        }

                //        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                //        return new ExternalAuthenticateResultModel
                //        {
                //            AccessToken = accessToken,
                //            EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                //            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                //        };
                //    }
                default:
                    {
                        throw _loginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }
            }
        }
        private async Task<LoginResult> GetLoginResultAsync(string username, string password, string tenancyName)
        {
            var loginResult = await _loginManager.LoginAsync(username, password, tenancyName);

            switch (loginResult.Result)
            {
                case LoginResultType.Success:
                    return loginResult;
                default:
                    throw _loginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, username, tenancyName);
            }
        }
        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }
        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (userInfo.ProviderKey != model.ProviderKey)
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }

            return userInfo;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }
        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }
        
    }
}
