using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicTools.Tools
{
    public class Helpers
    {
        #region [- GetRandomNumber(int length) ]
        public static long GetRandomNumber(int length)
        {
            return long.Parse(new string((from s in Enumerable.Repeat("123456789", length)
                                          select s[new Random().Next(9)]).ToArray()));
        }
        #endregion
    }
}
