using Master.Module;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Web.Components
{
    [HtmlTargetElement(Attributes ="module-button")]
    public class ModuleButtonTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public ModuleButton ModuleButton { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            return Task.Run(() => {
                SetAttributes(output);
                output.Content.AppendHtml(ModuleButton.ButtonName);

            });
        }

        private void SetAttributes(TagHelperOutput output)
        {
            if (ModuleButton.ButtonType.HasFlag(ButtonType.ForNoneRow))
            {
                output.Attributes.Add("fornonerow", "1");
            }

            output.Attributes.Add("type", "button");
            output.Attributes.Add("buttonname", ModuleButton.ButtonName);
            output.Attributes.Add("modulekey", ModuleButton.ModuleInfo?.ModuleKey);
            output.Attributes.Add("params", ModuleButton.ButtonActionParam);
            output.Attributes.Add("lay-event", ModuleButton.ButtonKey);
            output.Attributes.Add("confirmmsg", ModuleButton.ConfirmMsg);
            output.Attributes.Add("buttonactiontype", ModuleButton.ButtonActionType);
            output.Attributes.Add("buttonactionurl", ModuleButton.ButtonActionUrl);
            if (!string.IsNullOrEmpty(ModuleButton.ButtonScript))
            {
                output.Attributes.Add("onclick", ModuleButton.ButtonScript);
            }
            else
            {
                output.Attributes.Add("onclick", "func.callModuleButtonEvent()");
            }
            
        }
    }
}
