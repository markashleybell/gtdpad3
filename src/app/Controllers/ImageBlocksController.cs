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
    [Route("api/pages/{pageID}/imageblocks")]
    public class ImageBlocksController : ControllerBase<ImageBlocksController>
    {
        public ImageBlocksController(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<Settings> optionsMonitor,
            ILogger<ImageBlocksController> logger,
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
            var imageBlocks = await Repository.GetImageBlocks(pageID);

            return Ok(imageBlocks.Select(ImageBlockDTO.FromImageBlock));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid _, Guid id)
        {
            var section = await Repository.GetImageBlock(id);

            return section is null
                ? (IActionResult)NotFound()
                : Ok(ImageBlockDTO.FromImageBlock(section));
        }

        [HttpPost]
        public async Task<IActionResult> Post(ImageBlockDTO dto)
        {
            Guard.AgainstNull(dto, nameof(dto));

            var section = new ImageBlock(
                id: Guid.NewGuid(),
                page: dto.Page,
                owner: UserID,
                title: dto.Title,
                order: dto.Order
            );

            await Repository.PersistImageBlock(section);

            return CreatedAtAction(nameof(Get), dto.WithID(section.ID));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ImageBlockDTO dto)
        {
            Guard.AgainstNull(dto, nameof(dto));

            var section = await Repository.GetImageBlock(id);

            if (section is null)
            {
                return NotFound();
            }

            var updatedSection = section.With(
                title: dto.Title,
                order: dto.Order
            );

            await Repository.PersistImageBlock(updatedSection);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var list = await Repository.GetImageBlock(id);

            if (list is null)
            {
                return NotFound();
            }

            await Repository.DeleteImageBlock(id);

            return NoContent();
        }
    }
}
