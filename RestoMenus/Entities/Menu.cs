using RestoMenus.Models;

namespace RestoMenus.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public virtual Category Category { get; set; }
    }
}
