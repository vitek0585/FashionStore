namespace FashionStore.Models.BreadCrumbs
{
    public interface IBreadCrumbsModel
    {
        string NameLink { get; set; }
        string Href { get; set; }

    }

    public class BreadCrumbsModel : IBreadCrumbsModel
    {
        public string NameLink { get; set; }
        public string Href { get; set; }


    }

    public interface IBreadCrumbsCategoryModel : IBreadCrumbsModel
    {
        int CategoryId { get; set; }
        string TypeName { get; set; }
        string TypeHref { get; set; }

    }
}