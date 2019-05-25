using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http.Features;

namespace MvcMovie.Controllers
{
    public class HelloDockerController : Controller
    {
        // 
        // GET: /HelloDocker/

        public IActionResult Index()
        {
            ViewData["Message"] = "Hello Docker world!";
            IHttpConnectionFeature feature = HttpContext.Features.Get<IHttpConnectionFeature>();
            ViewData["IP"] = feature.LocalIpAddress.ToString();
            
            return View();
        }
    }
}