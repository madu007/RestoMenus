namespace RestoMenus.Entities
{
    public class Banner
    {
        public int Id { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Description { get; set; }

        public virtual List<BannerImage> BannerImages { get; set;}
    }
}
