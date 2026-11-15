using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class Proxy : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
