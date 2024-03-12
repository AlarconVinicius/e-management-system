using EMS.WebAPI.Core.Services.Notifications;

namespace EMS.WebAPI.Core.Services;

public interface INotifier
{
    bool HasNotification();
    List<Notification> GetNotifications();
    void Handle(Notification notification);
}