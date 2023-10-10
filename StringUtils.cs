using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public class StringUtils
    {
        public static bool EqualsIgnoreCase(string str1, string str2)
        {
            return string.Equals(str1, str2,StringComparison.OrdinalIgnoreCase);
        }
        public static bool ContainsIgnoreCase(string str1, string str2)
        {
            return str1?.IndexOf(str2, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
