namespace EMS.Core.Responses.Dashboards;

public class DashboardResponse
{
    public int TotalClients { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalServices { get; set; }
    public int TotalAppointments { get; set; }
    public List<AppointmentRetentionDataResult> AppointmentRetention { get; set; }
    public List<int> EmployeeStatus { get; set; }
    public List<int> AvailableYears { get; set; }

    public DashboardResponse() { }

    public DashboardResponse(int totalClients, int totalEmployees, int totalServices, int totalAppointments, List<AppointmentRetentionDataResult> appointmentRetention, List<int> employeeStatus, List<int> availableYears)
    {
        TotalClients = totalClients;
        TotalEmployees = totalEmployees;
        TotalServices = totalServices;
        TotalAppointments = totalAppointments;
        AppointmentRetention = appointmentRetention;
        EmployeeStatus = employeeStatus;
        AvailableYears = availableYears;
    }
}

public class AppointmentRetentionDataResult
{
    public string Month { get; set; }
    public int Realized { get; set; }
    public int Canceled { get; set; }

    public AppointmentRetentionDataResult() { }

    public AppointmentRetentionDataResult(string month, int realized, int canceled)
    {
        Month = month;
        Realized = realized;
        Canceled = canceled;
    }
}