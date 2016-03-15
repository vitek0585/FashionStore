namespace FashionStore.WorkFlow.BreadCrumbs.Tag.Interfaces
{
    public interface ITagCreator
    {
        string CreateTag(string textHtml,string href);
        string CreateEndTag(string textHtml);
    }
}