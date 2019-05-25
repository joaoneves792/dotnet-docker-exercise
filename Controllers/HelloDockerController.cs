using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class HelloDockerController : Controller
    {
        // 
        // GET: /HelloDocker/

        public IActionResult Index()
        {
            ViewData["Message"] = "Hello Docker world!";
            return View();
        }
    }
}