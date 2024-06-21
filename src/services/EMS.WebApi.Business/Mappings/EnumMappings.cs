using EMS.Core.Enums;
using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Mappings;

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

    public static EServiceStatusCore MapEServiceStatusToEServiceStatusCore(this EServiceStatus role)
    {
        return role switch
        {
            EServiceStatus.Scheduled => EServiceStatusCore.Scheduled,
            EServiceStatus.InProgress => EServiceStatusCore.InProgress,
            EServiceStatus.Completed => EServiceStatusCore.Completed,
            EServiceStatus.Cancelled => EServiceStatusCore.Cancelled,
            _ => EServiceStatusCore.Scheduled
        };
    }

    public static EServiceStatus MapEServiceStatusCoreToEServiceStatus(this EServiceStatusCore role)
    {
        return role switch
        {
            EServiceStatusCore.Scheduled => EServiceStatus.Scheduled,
            EServiceStatusCore.InProgress => EServiceStatus.InProgress,
            EServiceStatusCore.Completed => EServiceStatus.Completed,
            EServiceStatusCore.Cancelled => EServiceStatus.Cancelled,
            _ => EServiceStatus.Scheduled
        };
    }
}