using RestoMenus.Data;
using RestoMenus.Entities;
using RestoMenus.Models;
using System.ComponentModel.DataAnnotations;

namespace RestoMenus.Services
{
    public interface IMenuService
    {
        List<Menu> GetAllMenus();
        Menu GetMenuById(int id);
        Task AddMenu(MenuModel menuModel);
        List<Menu> GetBreakFast();
        List<Menu> GetLunch();
        List<Menu> GetDinner();
    }
    public class MenuService : IMenuService
    {
        private readonly RestoMenusContext _context;
        private readonly IWebHostEnvironment _environment;

        public MenuService(RestoMenusContext context, IWebHostEnvironment environment) 
        { 
            _context = context;
            _environment = environment;
        }

        public List<Menu> GetAllMenus()
        {
            var menuList = _context.Menus
                .OrderBy(m => m.Id)
                .ToList();

            return menuList;
        }

        public Menu GetMenuById(int id)
        {
            var menu = _context.Menus.Find(id);
            if (menu == null)
            {
                throw new ValidationException("Id is required");
            }

            return menu;
        }


        public async Task AddMenu(MenuModel menuModel)
        {
            // Create a list to store the file paths
            List<string> imagePaths = new List<string>();

            // Handle the uploaded images
            if (menuModel.Images != null && menuModel.Images.Count > 0) 
            { 
                foreach (var imageFile in menuModel.Images)
                {
                    if(imageFile != null  && imageFile.Length > 0)                 
                    {
                        //Save the image to a location of your choice e.g folder
                        // You can generate a uniqu file name for the image
                        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(_environment.WebRootPath + "/Images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        //Store the file path in your database for reference

                        imagePaths.Add("Images/" + fileName);
                    }
                }
            }

            // Save Menu to database
            var MenuEntity = new Menu
            {
                Id = menuModel.Id,
                Title = menuModel.Title,
                Description = menuModel.Description,
                Price = menuModel.Price,
                Category = _context.Categories.Find(menuModel.SelectedCategoryId)
            };

            _context.Menus.Add(MenuEntity);
            _context.SaveChanges();


            // The uploaded image file with the menu
            if (imagePaths.Count > 0)
            {
                foreach(var imagePath in imagePaths)
                {
                    var imageEntity = new MenuImage
                    {
                        MenuId = menuModel.Id,
                        ImagePaths = imagePath
                    };
                    _context.MenuImages.Add(imageEntity);
                }

                _context.SaveChanges();
            }
        }

        public List<Menu> GetBreakFast()
        {
            var breakFast = _context.Menus
                .Where(m => m.Category.Title == "Breakfast")
                .OrderBy(m => m.Id)
                .ToList();

            return breakFast;
        }

        public List<Menu> GetLunch()
        {
            var lunch = _context.Menus
                .Where(m => m.Category.Title == "Lunch")
                .OrderBy(m => m.Id)
                .ToList();

            return lunch;
        }

        public List<Menu> GetDinner()
        {
            var dinner = _context.Menus
                .Where(m => m.Category.Title == "Dinner")
                .OrderBy(m => m.Id)
                .ToList();

            return dinner;
        }
    }

}
