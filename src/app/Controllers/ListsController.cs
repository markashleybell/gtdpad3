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
    [Route("api/pages/{pageID}/lists")]
    public class ListsController : ControllerBase<ListsController>
    {
        public ListsController(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<Settings> optionsMonitor,
            ILogger<ListsController> logger,
            IRepository repository)
            : base(
                httpContextAccessor,
                optionsMonitor,
                logger,
                repository)
        { }

        [HttpGet]
        public async Task<IActionResult> Get(Guid pageID)
        {
            var lists = await Repository.GetLists(pageID);

            return Ok(lists.Select(ListDTO.FromList));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid _, Guid id)
        {
            var section = await Repository.GetList(id);

            return section is null
                ? (IActionResult)NotFound()
                : Ok(ListDTO.FromList(section));
        }

        [HttpPost]
        public async Task<IActionResult> Post(ListDTO dto)
        {
            Guard.AgainstNull(dto, nameof(dto));

            var section = new List(
                id: Guid.NewGuid(),
                page: dto.Page,
                owner: UserID,
                title: dto.Title,
                order: dto.Order
            );

            await Repository.PersistList(section);

            return CreatedAtAction(nameof(Get), dto.WithID(section.ID));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ListDTO dto)
        {
            Guard.AgainstNull(dto, nameof(dto));

            if (dto.ID != id)
            {
                return BadRequest();
            }

            var section = await Repository.GetList(id);

            if (section is null)
            {
                return NotFound();
            }

            var updatedSection = section.With(
                title: dto.Title,
                order: dto.Order
            );

            await Repository.PersistList(updatedSection);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var list = await Repository.GetList(id);

            if (list is null)
            {
                return NotFound();
            }

            await Repository.DeleteList(id);

            return NoContent();
        }
    }
}
