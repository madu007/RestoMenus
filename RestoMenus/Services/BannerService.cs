using RestoMenus.Data;
using RestoMenus.Entities;

namespace RestoMenus.Services
{
    public interface IBannerService
    {
        List<Banner> GetALLBanner();
    }
    public class BannerService : IBannerService
    {
        private readonly RestoMenusContext _context;

        public BannerService(RestoMenusContext context)
        {
            _context = context;
        }

        public List<Banner> GetALLBanner()
        {
            var bannerList = _context.Banner
                .OrderBy(b => b.Id)
                .ToList();

            bannerList = bannerList.Select(b =>
            {
                b.BannerImages = _context.BannerImages.Where(pi => pi.BannerId == b.Id).ToList();
                return b;
            }).ToList();

            return bannerList;
        }
    }
}
