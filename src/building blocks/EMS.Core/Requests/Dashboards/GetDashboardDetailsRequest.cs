namespace EMS.Core.Requests.Dashboards;

public class GetDashboardDetailsRequest : CompaniesRequest
{
    public int SelectedYear { get; set; }
    public GetDashboardDetailsRequest() { }
    public GetDashboardDetailsRequest(int selectedYear) { SelectedYear = selectedYear; }

}
