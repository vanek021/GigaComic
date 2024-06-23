using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string BreakByMaxLength(this string str, int maxLength)
        {
            if (str.Length > maxLength)
                return str.Substring(0, maxLength - 3) + "...";

            return str;
        }
    }
}
