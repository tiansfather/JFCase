using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES
{
    public class Const
    {
        /// <summary>
        /// 标准加工工艺库
        /// </summary>
        public static List<string> StandardProessTypes = new List<string>() {
            "数控铣","深孔钻","高速铣","电火花","精雕","线切割","高速精雕","钻床","磨床","飞刀","精飞","镗床","石墨精雕","锯床","激光焊","中走丝","慢走丝","大型卧铣","车床","电炉","调质","电镀","石墨", "抛光","热处理","氮化","冲床","模流分析","热流道","快速成型","激光雕刻","其他"
        };
    }
}
