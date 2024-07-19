using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDomain.Aggregates.JWT
{
    public class BlackListToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
