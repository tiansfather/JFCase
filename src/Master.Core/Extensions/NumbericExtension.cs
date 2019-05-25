using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Extensions
{
    public static class NumbericExtension
    {
        /// <summary>
        /// 判断一个浮点数是否没有小数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsInteger(this decimal number)
        {
            return number % 1 == 0;
        }
    }
}
