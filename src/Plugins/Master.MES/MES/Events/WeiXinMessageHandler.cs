using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Runtime.Session;
using Master.Authentication;
using Master.Configuration;
using Master.WeiXin.MessageHandlers.CustomMessageHandler;
using Microsoft.AspNetCore.Identity;
using Senparc.NeuChar.Entities;
using Senparc.NeuChar.Entities.Request;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Master.MES.Events
{
    /// <summary>
    /// 微信消息处理类
    /// </summary>
    public class WeiXinMessageHandler : CustomMessageHandler
    {
        public WeiXinMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0) : base(inputStream, postModel, maxRecordCount)
        {
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var defaultResponseMessage = base.CreateResponseMessage<ResponseMessageText>();
            
            using (IocManager.Instance.CreateScope())
            {
                var projectManager = IocManager.Instance.Resolve<MESProjectManager>();
                var fileManager = IocManager.Instance.Resolve<IFileManager>();
                var userManager = IocManager.Instance.Resolve<UserManager>();
                var startupConfiguration = IocManager.Instance.Resolve<IAbpStartupConfiguration>();
                var abpSession = IocManager.Instance.Resolve<IAbpSession>();

                var user = GetCurrentUser(requestMessage);
                var baseUrl = startupConfiguration.Modules.WebCore().BaseUrl;//基url,如http://demo.imould.me
                if (user == null)
                {
                   
                    defaultResponseMessage.Content = "当前用户未在系统中绑定";
                    return defaultResponseMessage;
                }
                //模拟账套数据查询
                using (abpSession.Use(user.TenantId, user.Id))
                {
                    var requestHandler =
                    requestMessage.StartHandler()
                    .Keyword("交接", () =>
                    {
                        //操作工设备交接
                        var equipmentManager = IocManager.Instance.Resolve<EquipmentManager>();
                        var equipments = equipmentManager.GetByOperatorId(user.Id).Result;
                        if (equipments.Count == 0)
                        {
                            defaultResponseMessage.Content = "您没有正在操作的设备，不需要交接" ;
                            return defaultResponseMessage;
                        }
                        else
                        {
                            var responseMessage = CreateResponseMessage<ResponseMessageNews>();
                            responseMessage.Articles.Add(new Article()
                            {
                                Title = "点击链接进行设备交接",
                                Url= $"{baseUrl}/MES/EquipmentTranisitionCode"
                            });

                            return responseMessage;
                        }
                        
                        
                    })
                    .Default(() =>
                    {
                        #region 寻找模具信息返回
                        //寻找匹配的模具
                        var project = projectManager.GetAll()
                            .Where(o => o.ProjectSN.Contains(requestMessage.Content))
                            .OrderByDescending(o => o.Id)
                            .FirstOrDefault();

                        if (project == null)
                        {
                            defaultResponseMessage.Content = "未找到模具信息";
                            return defaultResponseMessage;
                        }
                        var responseMessage = CreateResponseMessage<ResponseMessageNews>();

                        
                        var fileUrl = "";
                        if (project.ProjectPic.HasValue)
                        {
                            fileUrl = baseUrl + fileManager.GetFileUrl(project.ProjectPic.Value, 100, 100);
                        }

                        responseMessage.Articles.Add(new Article()
                        {
                            Title = project.ProjectSN,
                            Description = project.ProjectName,
                            PicUrl = fileUrl,
                            Url = $"{baseUrl}/mes/ProcessTaskWechat#/charger/project/user?projectId={project.Id}"
                        });

                        return responseMessage;
                        #endregion

                    });

                    return requestHandler.GetResponseMessage() as IResponseMessageBase;
                }
                

            }
        }

        #region 关注事件
        /// <summary>
        /// 关注后事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {

            //如果关注前是通过访问某个页面进来,则重定向至此页
            if (TemplateMessageCollection.ContainsKey(requestMessage.FromUserName))
            {
                var url = TemplateMessageCollection[requestMessage.FromUserName];

                var responseMessage = CreateResponseMessage<ResponseMessageNews>();

                responseMessage.Articles.Add(new Article()
                {
                    Title = "欢迎关注模来模往",
                    Description = GetDescriptionFromUrl(url),
                    Url = url
                });
                TemplateMessageCollection.Remove(requestMessage.FromUserName);
                return responseMessage;

            }
            else
            {
                var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
                responseMessage.Content = "欢迎关注模来模往。";
                return responseMessage;
            }


        }

        private string GetDescriptionFromUrl(string url)
        {
            if (url.ToLower().IndexOf("register") > 0)
            {
                return "请点击进行注册或领取";
            }else if (url.ToLower().IndexOf("bindremind") > 0)
            {
                return "请点击进行策略绑定";
            }
            else
            {
                return "请点击重新进行之前操作";
            }
        } 
        #endregion
    }
}
