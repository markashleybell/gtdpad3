using System.Linq;
using System.Threading.Tasks;
using GTDPad.Models;
using GTDPad.Services;
using GTDPad.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GTDPad.Controllers
{
    public class HomeController : ControllerBase<HomeController>
    {
        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<Settings> optionsMonitor,
            ILogger<HomeController> logger,
            IRepository repository)
            : base(
                httpContextAccessor,
                optionsMonitor,
                logger,
                repository)
        { }

        public async Task<IActionResult> Index()
        {
            var pages = await Repository.GetPages(UserID);

            var model = new IndexViewModel {
                Pages = pages,
                Page = pages.First()
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Routes() => View();
    }
}
