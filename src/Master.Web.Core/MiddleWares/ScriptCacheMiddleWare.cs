using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Web.MiddleWares
{
    
    public static class ScriptCacheMiddleWare
    {
        /// <summary>
        /// 用于緩存abp动态代理js
        /// </summary>
        public static IApplicationBuilder UseScriptCacheMiddleware(this IApplicationBuilder app)
        {
            return app.Use(async (ctx, next) =>
            {
                var filePath = ctx.Request.Path;
                string scripts;
                if (filePath.Value.ToLower() == "/scripts/getall.js")
                {
                    scripts = Senparc.CO2NET.HttpUtility.RequestUtility.HttpGet($"{ctx.Request.Scheme}://{ctx.Request.Host}/AbpServiceProxies/GetAll",Encoding.Default);
                    ctx.Response.Body.Write(Encoding.Default.GetBytes(scripts));
                    //ctx.Response.Redirect("/AbpServiceProxies/GetAll");
                }
                else if (filePath.Value.ToLower() == "/scripts/getscripts.js")
                {
                    scripts = Senparc.CO2NET.HttpUtility.RequestUtility.HttpGet($"{ctx.Request.Scheme}://{ctx.Request.Host}/AbpScripts/GetScripts", Encoding.Default);
                    ctx.Response.Body.Write(Encoding.Default.GetBytes(scripts));
                    //ctx.Response.Redirect("/AbpScripts/GetScripts");
                }
                else
                {
                    await next();
                }


            });
        }
    }
}
