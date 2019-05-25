using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Entity
{
    public interface IHaveStatus
    {
        //JsonObject<List<string>> Status { get; set; }
        string Status { get; set; }
    }

    public static class HaveStatusObjectExtension
    {
        public static List<string> AllStatus(this IHaveStatus haveStatusObject)
        {
            return string.IsNullOrEmpty(haveStatusObject.Status) ? new List<string>() : Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(haveStatusObject.Status);
            //return haveStatusObject.Status?.Object ?? new List<string>();
        }
        public static void SetStatus(this IHaveStatus haveStatusObject, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var allStatus = haveStatusObject.AllStatus();
                if (!allStatus.Contains(name))
                {
                    allStatus.Add(name);
                }
                haveStatusObject.Status =Newtonsoft.Json.JsonConvert.SerializeObject(allStatus);
            }
        }

        public static void RemoveStatus(this IHaveStatus haveStatusObject, string name)
        {
            var allStatus = haveStatusObject.AllStatus();
            if (allStatus.Contains(name))
            {
                allStatus.Remove(name);
            }

            haveStatusObject.Status = Newtonsoft.Json.JsonConvert.SerializeObject(allStatus);
        }

        public static bool HasStatus(this IHaveStatus haveStatusObject, string name)
        {
            try
            {
                //当Status为null时使用下句会报错
                //if(haveStatusObject.Status==null)
                return AllStatus(haveStatusObject).Contains(name);
            }
            catch
            {
                return false;
            }
            
        }
    }
}
