using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Web.MiddleWares
{
    /// <summary>
    /// 产生图片缩略图的中间件
    /// </summary>
    public static class ThumbMiddleware
    {
        public static IApplicationBuilder UseThumbMiddleware(this IApplicationBuilder app)
        {
            return app.Use(async (ctx, next) =>
            {
                var filePath = ctx.Request.Path;
                if (Common.ImageHelper.IsImg(filePath) && ctx.Request.Query.ContainsKey("w"))
                {
                    int.TryParse(ctx.Request.Query["w"], out var w);
                    var gap = true;//默认补足空白
                    if (ctx.Request.Query.ContainsKey("gap"))
                    {
                        Boolean.TryParse(ctx.Request.Query["gap"], out gap);
                    }
                    var stream = Common.ImageHelper.ThumbImageToStream(Common.PathHelper.VirtualPathToAbsolutePath(filePath), w, w, gap);
                    ctx.Response.ContentType = "image/png";
                    ctx.Response.ContentLength = stream.Length;
                    await ctx.Response.Body.WriteAsync(stream.ToArray());
                }
                else
                {
                    await next();
                }
                

            });
        }
    }
}
