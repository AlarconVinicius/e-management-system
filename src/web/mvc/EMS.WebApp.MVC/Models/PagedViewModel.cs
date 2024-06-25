namespace EMS.WebApp.MVC.Models;

public class PagedViewModel<T> : IPagedList where T : class
{
    public string ReferenceAction { get; set; }
    public IEnumerable<T> List { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string Query { get; set; }
    public int TotalResults { get; set; }
    public double TotalPages => Math.Ceiling((double)TotalResults / PageSize);
    public int FirstItemIndex => (PageIndex - 1) * PageSize + 1;
    public int LastItemIndex => Math.Min(PageIndex * PageSize, TotalResults);
}
public interface IPagedList
{
    public string ReferenceAction { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string Query { get; set; }
    public int TotalResults { get; set; }
    public double TotalPages { get; }
    public int FirstItemIndex { get; }
    public int LastItemIndex { get; }
}