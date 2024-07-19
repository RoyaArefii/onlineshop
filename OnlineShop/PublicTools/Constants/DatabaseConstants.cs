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
            public const string GodAadminId = "7b43aed3-cbdc-4fa2-9516-9e99d3e5dbbo";
            public const string GodAadminName = "GodAdmin";
            public const string GodAadminNormalizedName = "GODADMIN";
            public const string GodAadminCuncurencyStamp = "1";

            public const string AdminId = "7b43aed3-cbdc-4fa2-9516-9e99d3e5dbbl";
            public const string AdminName = "Admin";
            public const string AdminNormalizedName = "ADMIN";
            public const string AdminCuncurencyStamp = "2";

            public const string SupportId = "7b43aed3-cbdc-4fa2-9516-9e99d3e5dbbp";
            public const string SupportName = "Support";
            public const string SupportNormalizedName = "SUPPORT";
            public const string SupportCuncurencyStamp = "3";

            public const string NormalId = "7b43aed3-cbdc-4fa2-9516-9e99d3e5dbmm";
            public const string NormalName = "Normal";
            public const string NormalNormalizedName = "NORMAL";
            public const string NormalCuncurencyStamp = "4";

            public const string SellerId = "7b43aed3-cbdc-4fa2-9516-9e99d3e5dbml";
            public const string SellerName = "Seller";
            public const string SellerNormalizedName = "SELLER";
            public const string SellerCuncurencyStamp = "5";

            public const string BuyerId = "7b43aed3-cbdc-4fa2-9516-9e99d3e5dbmk";
            public const string BuyerName = "Buyer";
            public const string BuyerNormalizedName = "BUYER";
            public const string BuyerCuncurencyStamp = "6";
        }
        public static class GodAdminUsers
        {
            public const string ArefiUserId = "7b43aed3-cbdc-4fa2-9516-9e99d3e5dbbn";
            public const string ArefiFirstName = "Roya";
            public const string ArefiLastName = "Arefi";
            public const string ArefiCellPhone = "09378748824";
            public const string ArefiPassword = "!QAZ1qaz";
        }
    }
}
