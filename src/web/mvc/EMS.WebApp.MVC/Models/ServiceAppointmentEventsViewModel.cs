namespace EMS.WebApp.MVC.Models;

public class ServiceAppointmentEventsViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Color { get; set; }

    public ServiceAppointmentEventsViewModel() { }

    public ServiceAppointmentEventsViewModel(Guid id, string title, DateTime start, DateTime end, string color)
    {
        Id = id;
        Title = title;
        Start = start;
        End = end;
        Color = color;
    }
}
