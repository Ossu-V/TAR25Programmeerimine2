using Microsoft.AspNetCore.Mvc;

namespace ShopTARpe25.Controllers
{
    public class SpaceshipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
