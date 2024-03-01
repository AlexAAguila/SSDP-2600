namespace WildPath.ViewModels
{
    public class ProductVM
    {
        public WildPath.EfModels.Item Item { get; set; }
        public IEnumerable<WildPath.EfModels.ImageStore> ImageStore { get; set; }
    }
}
