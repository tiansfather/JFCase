using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Master.Web.Components
{
    public class EnumsTagHelper : TagHelper
    {
        public Enum Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var list = GetEnumSelectListItem();

            output.Content.AppendHtml("<select>");
            foreach (var item in list)
            {
                if (item.Value != null)
                    output.Content.AppendHtml($"<option value='{item.Value}'>{item.Text}</option>");
                else
                    output.Content.AppendHtml($"<option>{item.Text}</option>");
            }
            output.Content.AppendHtml("<select/>");
        }

        public List<SelectListItem> GetEnumSelectListItem()
        {
            var list = new List<SelectListItem>();
            var typeInfo = Value.GetType().GetTypeInfo();
            var enumValues = typeInfo.GetEnumValues();

            foreach (var value in enumValues)
            {

                MemberInfo memberInfo =
                    typeInfo.GetMember(value.ToString()).First();

                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                list.Add(new SelectListItem()
                {
                    Text = descriptionAttribute.Description,
                    Value = value.ToString()
                });
            }

            return list;
        }
    }
}
