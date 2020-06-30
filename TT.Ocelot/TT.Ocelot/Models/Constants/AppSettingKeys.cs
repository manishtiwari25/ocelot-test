using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TT.Ocelot.Models.Constants
{
    public class AppSettingKeys
    {
        public const string Authority = "Identity:Authority";
        public const string RequireHttpsMetadata = "Identity:Authority";
        public const string ApiSecret = "Identity:ApiSecret";
        public const string ApiName = "Identity:ApiName";
        public const string SecretForInternal = "Identity:SecretForInternal";
        public const string EnableOutsideIdentityServer = "Identity:EnableOutsideIdentityServer";

        public const string AdministrationPath = "GlobalConfiguration:AdministrationPath";
        public const string RequestIdKey = "GlobalConfiguration:RequestIdKey";

        public const string MySqlConnectionString = "ConnectionStrings:MySql";
    }
}
