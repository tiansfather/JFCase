using System;
using System.Collections.Generic;
using System.Text;

namespace SkyNet.Master.Dto
{
    public class PermissionDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string PermissionType
        {
            get
            {
                if (Name.Contains("Button"))
                {
                    return "按钮";
                }else if (Name.Contains("Field"))
                {
                    return "字段";
                }
                else
                {
                    return "";
                }
            }
        }
        public bool IsGranted { get; set; }
        public string ModuleName { get; set; }
    }

    
}
