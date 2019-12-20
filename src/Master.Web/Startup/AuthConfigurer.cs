using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Abp.Runtime.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Master.Web.Startup
{
    /// <summary>
    /// 用于在StartUp中配置授权认证信息
    /// </summary>
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            if (bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; 
                }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = new PathString("/Account/Forbidden/");
                    options.LoginPath = new PathString("/Home/Index/");
                    //options.Cookie.Name = "AuthCookie";
                    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                })
                .AddJwtBearer(options =>
                {
                    options.Audience = configuration["Authentication:JwtBearer:Audience"];
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // The signing key must match!
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                        // Validate the JWT Issuer (iss) claim
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                        // Validate the JWT Audience (aud) claim
                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                        // Validate the token expiry
                        ValidateLifetime = true,

                        // If you want to allow a certain amount of clock drift, set that here
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = TokenResolver
                    };
                });
            }
        }
        /// <summary>
        /// 用于从QueryString以及cookie中读取token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task TokenResolver(MessageReceivedContext context)
        {
            try
            {
                if (context.Request.Query.ContainsKey("token"))
                {
                    
                    //解密后才是真正的token
                    context.Token = SimpleStringCipher.Instance.Decrypt(context.Request.Query["token"], AppConsts.DefaultPassPhrase);
                    //query中有token的，直接写入到cookies中
                    context.Response.Cookies.Append("token", context.Request.Query["token"]);
                }
                if (context.Request.Cookies.ContainsKey("token"))
                {
                    //解密后才是真正的token
                    context.Token = SimpleStringCipher.Instance.Decrypt(context.Request.Cookies["token"], AppConsts.DefaultPassPhrase);
                }
                
            }
            catch
            {

            }
            return Task.CompletedTask;
        }
        /* This method is needed to authorize SignalR javascript client.
         * SignalR can not send authorization header. So, we are getting it from query string as an encrypted text. */
        private static Task QueryStringTokenResolver(MessageReceivedContext context)
        {
            if (!context.HttpContext.Request.Path.HasValue ||
                !context.HttpContext.Request.Path.Value.StartsWith("/signalr"))
            {
                // We are just looking for signalr clients
                return Task.CompletedTask;
            }

            var qsAuthToken = context.HttpContext.Request.Query["enc_auth_token"].FirstOrDefault();
            if (qsAuthToken == null)
            {
                // Cookie value does not matches to querystring value
                return Task.CompletedTask;
            }

            // Set auth token from cookie
            context.Token = SimpleStringCipher.Instance.Decrypt(qsAuthToken, AppConsts.DefaultPassPhrase);
            return Task.CompletedTask;
        }
    }
}
