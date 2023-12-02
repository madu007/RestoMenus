using RestoMenus.Data;
using RestoMenus.Entities;

namespace RestoMenus.Services
{
    public interface IMenuImageService
    {
        List<MenuImage> GetMenuImages();
    }
    public class MenuImageService : IMenuImageService
    {
        private readonly RestoMenusContext _context;

        public MenuImageService(RestoMenusContext context) 
        {
            _context = context;
        }

        public List<MenuImage> GetMenuImages() 
        { 
            var menuImageList = _context.MenuImages
                .OrderBy(m => m.Id)
                .ToList();

            return menuImageList;
        }
    }
}
