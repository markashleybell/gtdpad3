using System.Threading.Tasks;
using GTDPad.Models;
using GTDPad.Services;
using GTDPad.Support;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static GTDPad.Constants;

namespace GTDPad.Controllers
{
    public class UsersController : ControllerBase<UsersController>
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserService _userService;

        public UsersController(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<Settings> optionsMonitor,
            ILogger<UsersController> logger,
            IRepository repository,
            IDateTimeService dateTimeService,
            IUserService userService)
            : base(
                httpContextAccessor,
                optionsMonitor,
                logger,
                repository)
        {
            _dateTimeService = dateTimeService;
            _userService = userService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Guard.AgainstNull(model, nameof(model));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (valid, id) = await _userService.ValidateLogin(model.Email, model.Password);

            if (!valid)
            {
                return View(model);
            }

            var principal = _userService.GetClaimsPrincipal(id.Value, model.Email);

            var authenticationProperties = new AuthenticationProperties {
                IsPersistent = true,
                ExpiresUtc = _dateTimeService.Now.AddDays(Settings.PersistentSessionLengthInDays)
            };

            await HttpContextAccessor.HttpContext.SignInAsync(principal, authenticationProperties);

            return Redirect(SiteIndexUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContextAccessor.HttpContext.SignOutAsync();

            return Redirect(SiteIndexUrl);
        }
    }
}
