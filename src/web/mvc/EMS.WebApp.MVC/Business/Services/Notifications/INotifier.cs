namespace EMS.WebApp.MVC.Business.Services.Notifications;

public interface INotifier
{
    bool HasNotification();
    List<Notification> GetNotifications();
    void Handle(Notification notification);
}