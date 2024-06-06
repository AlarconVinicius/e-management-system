namespace EMS.Core.Configuration;

public static class ConfigurationDefault
{
    public const int DefaultStatusCode = 200;
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 25;

    public static string BackendUrl { get; set; } = "http://localhost:5036";
    public static string FrontendUrl { get; set; } = "http://localhost:5124";

}