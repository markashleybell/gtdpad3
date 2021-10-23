using System;
using System.Linq;
using System.Threading.Tasks;
using GTDPad.Domain;
using GTDPad.DTO;
using GTDPad.Services;
using GTDPad.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GTDPad.Controllers
{
    public class PagesController : ControllerBase<PagesController>
    {
        public PagesController(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<Settings> optionsMonitor,
            ILogger<PagesController> logger,
            IRepository repository)
            : base(
                httpContextAccessor,
                optionsMonitor,
                logger,
                repository)
        { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pages = await Repository.GetPages(UserID);

            return Ok(pages.Select(PageDTO.FromPage));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var page = await Repository.GetPage(id);

            return page is null
                ? (IActionResult)NotFound()
                : Ok(PageDTO.FromPage(page));
        }

        [HttpPost]
        public async Task<IActionResult> Post(PageDTO dto)
        {
            Guard.AgainstNull(dto, nameof(dto));

            var page = new Page(
                id: Guid.NewGuid(),
                owner: UserID,
                title: dto.Title,
                slug: dto.Slug,
                order: dto.Order
            );

            await Repository.PersistPage(page);

            return CreatedAtAction(nameof(Get), new { id = page.ID }, dto.WithID(page.ID));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, PageDTO dto)
        {
            Guard.AgainstNull(dto, nameof(dto));

            var page = await Repository.GetPage(id);

            if (page is null)
            {
                return NotFound();
            }

            var updatedPage = page.With(
                title: dto.Title,
                slug: dto.Slug,
                order: dto.Order
            );

            await Repository.PersistPage(updatedPage);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var page = await Repository.GetPage(id);

            if (page is null)
            {
                return NotFound();
            }

            await Repository.DeletePage(id);

            return NoContent();
        }
    }
}
