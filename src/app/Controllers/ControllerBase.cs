using System;
using System.Security.Claims;
using GTDPad.Services;
using GTDPad.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GTDPad.Controllers
{
    [Authorize]
    public class ControllerBase<TController> : Controller
    {
        public ControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<Settings> optionsMonitor,
            ILogger<TController> logger,
            IRepository repository)
        {
            Guard.AgainstNull(httpContextAccessor, nameof(httpContextAccessor));
            Guard.AgainstNull(optionsMonitor, nameof(optionsMonitor));
            Guard.AgainstNull(logger, nameof(logger));
            Guard.AgainstNull(repository, nameof(repository));

            HttpContextAccessor = httpContextAccessor;
            Settings = optionsMonitor.CurrentValue;
            Logger = logger;
            Repository = repository;
        }

        protected ILogger<TController> Logger { get; }

        protected IHttpContextAccessor HttpContextAccessor { get; }

        protected IRepository Repository { get; }

        protected Settings Settings { get; }

        protected Guid UserID
        {
            get
            {
                if (User?.Identity is null)
                {
                    return Guid.Empty;
                }

                var identity = User.Identity as ClaimsIdentity;

                var value = identity?.FindFirst(ClaimTypes.Sid)?.Value;

                return value is object ? new Guid(value) : Guid.Empty;
            }
        }
    }
}
