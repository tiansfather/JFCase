using hyjiacan.py4n;
using hyjiacan.py4n.format;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class PingYinHelper
    {
        public static string GetPinyin(string input)
        {
            var format=new PinyinOutputFormat(ToneFormat.WITHOUT_TONE, CaseFormat.LOWERCASE, VCharFormat.WITH_U_UNICODE);
            return Pinyin4Net.GetPinyin(input, format);
        }
    }
}
