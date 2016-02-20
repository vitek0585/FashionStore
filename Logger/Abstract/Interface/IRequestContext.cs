namespace WebLogger.Abstract.Interface
{
    public interface IRequestContext
    {
        string HttpMethod { get; }
        bool IsAuthenticated { get; }
        string Path { get; }
        string UrlReferrer { get; }
        string UserAgent { get; }
    }
}
