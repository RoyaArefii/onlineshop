using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicTools.Constants
{
    public static class DatabaseConstants
    {
        #region [- Schemas -]
        public static class Schemas
        {
            public const string Infrustructure = "Infrustructure";
            public const string Sale = "Sale";
            public const string UserManagement = "UserManagement";
        }
        #endregion
        public static class CheckConstraints
        {
            public enum ReturnValueTypes
            {
                ScriptTitles = 1,
                ScriptCodes = 2,
            }

            public static string SetOnlyNumericalCheckConstraint(string propertytTitle, ReturnValueTypes returnValueTypes)
            {

                return returnValueTypes switch
                {
                    (ReturnValueTypes)1 => $"Check_{propertytTitle}_OnlyNumerical",
                    (ReturnValueTypes)2 => $"Check_{propertytTitle} Not Like '%[^0-9]%'",
                    _ => string.Empty,
                };

            }
            public static string SetDigitLimitCheckConstraint(string propertytTitle, int digitLimit, ReturnValueTypes returnValueTypes)
            {
                var script = new StringBuilder();
                while (digitLimit != 0)
                {
                    script.Append("[0-9]");
                    digitLimit -= digitLimit;
                }
                return returnValueTypes switch
                {
                    (ReturnValueTypes)1 => $"Check_{propertytTitle}_DigitLimit",
                    (ReturnValueTypes)2 => $"Check_{propertytTitle}  Like '{script}'",
                    _ => string.Empty,
                };
            }
        }
        public static class DefaultRoles
        {
            public const string GodAadminId = "1";
            public const string GodAadminName = "GodAdmin";
            //public const string GodAadminNormalizedName = "GODADMIN";
            //public const string GodAadminCuncurencyStamp = "1";

            public const string AadminId = "2";
            public const string AadminName = "Admin";
            //public const string AadminNormalizedName = "ADMIN";
            //public const string AadminCuncurencyStamp = "2";

            public const string SupportId = "3";
            public const string SupportName = "Support";
            //public const string SupportNormalizedName = "SUPPORT";
            //public const string SupportCuncurencyStamp = "3";
        }
        public static class GodAdminUsers
        {
            public const string ArefiUserId = "1";
            public const string ArefiFirstName = "Roya";
            public const string ArefiLastName = "Arefi";
            public const string ArefiCellPhone = "09378748824";
            public const string ArefiPassword = "!QAZ1qaz";
        }
    }
}
