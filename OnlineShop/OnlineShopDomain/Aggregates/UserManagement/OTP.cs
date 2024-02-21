using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDomain.Aggregates.UserManagement
{
    public class OTP : IEntity<Guid>, ICreatedEntity, IDbSetEntity
    {
        public Guid Id { get ; set ; }

        public AppUserRole AppUserRole{ get; set; }
        public string Pincode { get; set; }
        public string TargetCellphone { get; set; }
       // public ActivityType ActivityType { get; set; }
        public TimeOnly ExpirationTime { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public bool ISUsed { get; set; }
        public TimeOnly UsedTime { get; set; }
        public DateOnly UsedDate { get; set; }
        public string HashedExtraData { get; set; }
        public string ExtraData { get; set; }
        public DateTime DateCreatedLatin { get; set; }
        public string DateCreatedPersian { get; set; }
    }
}
