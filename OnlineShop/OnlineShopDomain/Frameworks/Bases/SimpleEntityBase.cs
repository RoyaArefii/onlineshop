using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDomain.Frameworks.Bases
{
    internal class SimpleEntityBase : ISimpleEntity
    {
        public int Id { get ; set ; }
        public string Title { get ; set ; }
        public string EntityDescription { get; set ; }
        public bool IsActive { get; set ; }
    }
}
