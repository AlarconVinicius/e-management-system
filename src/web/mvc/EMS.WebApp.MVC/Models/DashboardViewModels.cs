namespace EMS.WebApp.MVC.Models;

public class DashboardViewModel
{
    public int TotalClientes { get; set; }
    public int TotalColaboradores { get; set; }
    public int TotalServicos { get; set; }
    public int TotalAgendamentos { get; set; }
    public List<AppointmentRetentionData> AppointmentRetention { get; set; }
    public List<int> StatusColaboradores { get; set; }
    public List<int> AnosDisponiveis { get; set; }
    public int AnoSelecionado { get; set; }
}

public class AppointmentRetentionData
{
    public string Mes { get; set; }
    public int Realizado { get; set; }
    public int Cancelado { get; set; }
}
