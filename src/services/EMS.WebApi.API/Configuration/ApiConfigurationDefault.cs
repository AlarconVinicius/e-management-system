namespace EMS.WebApi.API.Configuration;

public static class ApiConfigurationDefault
{
    public static string ConnectionString { get; set; } = string.Empty;
    public static string CorsPolicyName = "wasm";
}