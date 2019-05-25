using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    public class ValuePathParser : IValuePathParser
    {
        /// <summary>
        /// CreatorUser.Name=>CreatorUser!=null?CreatorUser.Name:null
        /// </summary>
        /// <param name="valuePath">CreatorUser.Name,A.B.C  注，层级不限</param>
        /// <returns>CreatorUser!=null?CreatorUser.Name:null</returns>
        public string Parse(string valuePath)
        {
            //列的值表达式解析 20180607 lijianbo
            var temparry = valuePath.Split('.');
            int lenght = temparry.Length;

            string rstr = "";

            if (lenght > 1)
            {
                rstr = temparry[0] + "!=null?" + Parse(valuePath, 1) + ":null";
            }
            else
            {
                rstr = valuePath;
            }

            //async.Select("new(Name as a,CreatorUser!=null?CreatorUser.Name:null as b)")
            return rstr;
        }
        public string Parse(string valuePath, int num)
        {
            var temparry = valuePath.Split('.');
            int lenght = temparry.Length;
            num++;
            string rstr = "";
            if (lenght > num)
            {
                string tempstr = "";
                for (var i = 0; i < num; i++)
                {
                    if (tempstr == "")
                    {
                        tempstr = temparry[i];
                    }
                    else
                    {
                        tempstr = tempstr + "." + temparry[i];
                    }
                }

                rstr = tempstr + "!=null?" + Parse(valuePath, num) + ":null";
            }
            else
            {
                rstr = valuePath;
            }

            return rstr;
        }
    }
}
