using EMS.Core.Enums;
using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Mappings;

public static class EnumMappings
{
    public static ERoleCore MapERoleToERoleCore(this ERole role)
    {
        return role switch
        {
            ERole.Admin => ERoleCore.Admin,
            ERole.Employee => ERoleCore.Employee,
            ERole.Client => ERoleCore.Client,
            _ => ERoleCore.Client
        };
    }

    public static ERole MapERoleCoreToERole(this ERoleCore role)
    {
        return role switch
        {
            ERoleCore.Admin => ERole.Admin,
            ERoleCore.Employee => ERole.Employee,
            ERoleCore.Client => ERole.Client,
            _ => ERole.Client
        };
    }
}