using Master.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class MESStatusNames
    {
        public const string ReceiveOuterTask= "ReceiveOuterTask";
    }
    public class MESUserStatusDefinitionProvider : IStatusProvider<User>
    {
        public IEnumerable<StatusDefinition> GetStatusDefinitions()
        {
            var result = new List<StatusDefinition>();
            result.Add(new StatusDefinition()
            {
                Name = "ProjectCharger",
                DisplayName = "模具组长",
                Tips = "标记后能看到模具的模具组长设置为此用户的模具"
            });
            result.Add(new StatusDefinition()
            {
                Name = "ProjectTracker",
                DisplayName = "项目负责",
                Tips = "标记后能看到模具的项目负责设置为此用户的模具"
            });
            result.Add(new StatusDefinition()
            {
                Name = "ProductDesign",
                DisplayName = "产品设计",
                Tips = "标记后能看到模具的产品设计设置为此用户的模具"
            });
            result.Add(new StatusDefinition()
            {
                Name = "MouldDesign",
                DisplayName = "模具设计",
                Tips = "标记后能看到模具的模具设计设置为此用户的模具"
            });
            result.Add(new StatusDefinition()
            {
                Name = "Salesman",
                DisplayName = "业务员",
                Tips = "标记后能看到模具的业务员设置为此用户的模具"
            });
            result.Add(new StatusDefinition()
            {
                Name="ReceiveOuterTask",
                DisplayName="往来消息接收",
                Tips="开启后可以收到外部账套发送过来的任务消息"
            });
            result.Add(new StatusDefinition()
            {
                Name = "ReceiveSendRemind",
                DisplayName = "开单发送提醒",
                Tips = "开启后当外协加工任务开单后会接收到提醒,需要发送权限"
            });
            return result;
        }
    }
}
