using Microsoft.AspNetCore.Mvc;
using RestoMenus.Models;
using RestoMenus.Services;
using System.Diagnostics;

namespace RestoMenus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMenuService _menuService;
        private readonly IMenuImageService _menuImageService;
        private readonly IBannerService _bannerService;

        public HomeController(ILogger<HomeController> logger, IMenuService menuService, IMenuImageService menuImageService, IBannerService bannerService)
        {
            _logger = logger;
            _menuService = menuService;
            _menuImageService = menuImageService;
            _bannerService = bannerService;
        }

        public IActionResult Index()
        {
            var breakFastMenu = _menuService.GetBreakFast();
            var lunchMenu = _menuService.GetLunch();
            var dinnerMenu = _menuService.GetDinner();

            // Map Breakfast menu entity to its model version

            var breakFastMenuModelList = breakFastMenu.Select(menu => new MenuModel
            {
                Id = menu.Id,
                Title = menu.Title,
                Description = menu.Description,
                Price = menu.Price,
                ImagePaths = _menuImageService.GetMenuImages().Where(m => m.MenuId == menu.Id).Select(c => c.ImagePaths).ToList(),
            }).ToList();

            // Map Lunch menu entity to its model version

            var lunchMenuModelList = lunchMenu.Select(menu => new MenuModel
            {
                Id = menu.Id,
                Title = menu.Title,
                Description = menu.Description,
                Price = menu.Price,
                ImagePaths = _menuImageService.GetMenuImages().Where(m => m.MenuId == menu.Id).Select(c => c.ImagePaths).ToList(),
            }).ToList();

            // Map Dinner menu entity to its model version

            var dinnertMenuModelList = dinnerMenu.Select(menu => new MenuModel
            {
                Id = menu.Id,
                Title = menu.Title,
                Description = menu.Description,
                Price = menu.Price,
                ImagePaths = _menuImageService.GetMenuImages().Where(m => m.MenuId == menu.Id).Select(c => c.ImagePaths).ToList(),
            }).ToList();

            // Get Banner

            var banner = _bannerService.GetALLBanner();

            var bannerModelList = banner.Select(banner => new BannerModel
            {
                Title1 = banner.Title1,
                Title2 = banner.Title2,
                Description = banner.Description,
                ImagePaths  =  banner.BannerImages.Select(b => b.ImagePath).ToList(),
            }).ToList();

            var landingPageModel = new LandingPageModel
            {
                BreakFastMenu = breakFastMenuModelList,
                LuchMenu = lunchMenuModelList,
                DinnerMenu = dinnertMenuModelList,
                Banners = bannerModelList
            };

            return View(landingPageModel);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Book()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}