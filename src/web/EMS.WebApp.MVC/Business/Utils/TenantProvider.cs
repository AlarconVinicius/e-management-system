namespace EMS.WebApp.MVC.Business.Utils;

public interface ITenantProvider
{
    void SetTenantId(string tenantId);
    string GetTenantId();
}
public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    private HttpContext HttpContext => _httpContextAccessor.HttpContext;

    public void SetTenantId(string tenantId)
    {
        HttpContext.Session.SetString("TenantId", tenantId);
    }

    public string GetTenantId()
    {
        //return HttpContext.Session.GetString("TenantId");
        return _httpContextAccessor.HttpContext?.Session.GetString("TenantId");
    }
}
