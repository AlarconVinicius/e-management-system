namespace EMS.WebApp.MVC.Business.Models.ViewModels;

public class ErrorViewModel
{
    public string RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}