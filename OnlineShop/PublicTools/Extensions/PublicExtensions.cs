using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PublicTools.Extensions
{
    public static class PublicExtensions
    {
        #region [- ExtractNumber(this string input) -]

        public static int ExtractNumber(this string input)
        {
            var result = Regex.Replace(input, "[^0-9 _]", "");
            return string.IsNullOrWhiteSpace(result) ? 0 : int.Parse(result);
        }

        #endregion
    }
}
