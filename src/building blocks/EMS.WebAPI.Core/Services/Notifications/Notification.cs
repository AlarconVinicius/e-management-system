namespace EMS.WebAPI.Core.Services.Notifications;

public class Notification
{
    public Notification(string message)
    {
        Message = message;
    }

    public string Message { get; }
}