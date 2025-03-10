namespace FinTrack.Web.Helpers;

public class PagedResponse<T>  where T : class
{
    public T Data { get; set; }
    public PaginationMetadata Pagination { get; set; }
}
