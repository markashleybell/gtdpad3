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
    [Route("api/pages/{pageID}/richtextblocks")]
    public class RichTextBlocksController : ControllerBase<RichTextBlocksController>
    {
        public RichTextBlocksController(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<Settings> optionsMonitor,
            ILogger<RichTextBlocksController> logger,
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
            var richTextBlocks = await Repository.GetRichTextBlocks(pageID);

            return Ok(richTextBlocks.Select(RichTextBlockDTO.FromRichTextBlock));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid _, Guid id)
        {
            var section = await Repository.GetRichTextBlock(id);

            return section is null
                ? (IActionResult)NotFound()
                : Ok(RichTextBlockDTO.FromRichTextBlock(section));
        }

        [HttpPost]
        public async Task<IActionResult> Post(RichTextBlockDTO dto)
        {
            Guard.AgainstNull(dto, nameof(dto));

            var section = new RichTextBlock(
                id: Guid.NewGuid(),
                page: dto.Page,
                owner: UserID,
                title: dto.Title,
                text: dto.Text,
                order: dto.Order
            );

            await Repository.PersistRichTextBlock(section);

            return CreatedAtAction(nameof(Get), dto.WithID(section.ID));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, RichTextBlockDTO dto)
        {
            Guard.AgainstNull(dto, nameof(dto));

            if (dto.ID != id)
            {
                return BadRequest();
            }

            var section = await Repository.GetRichTextBlock(id);

            if (section is null)
            {
                return NotFound();
            }

            var updatedSection = section.With(
                title: dto.Title,
                text: dto.Text,
                order: dto.Order
            );

            await Repository.PersistRichTextBlock(updatedSection);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var list = await Repository.GetRichTextBlock(id);

            if (list is null)
            {
                return NotFound();
            }

            await Repository.DeleteRichTextBlock(id);

            return NoContent();
        }
    }
}
