﻿namespace EMS.Core.Notifications;

public class Notifier : INotifier
{
    private List<Notification> _notifications;

    public Notifier()
    {
        _notifications = new List<Notification>();
    }

    public void Handle(Notification notification)
    {
        _notifications.Add(notification);
    }
    public void Clear()
    {
        _notifications.Clear();
    }
    public List<Notification> GetNotifications()
    {
        return _notifications;
    }

    public bool HasNotification()
    {
        return _notifications.Any();
    }
}