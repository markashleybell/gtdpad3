using System;
using GTDPad.Domain;

namespace GTDPad.DTO
{
    public class ImageBlockDTO : DTOBase<ImageBlockDTO>
    {
        public Guid Page { get; set; }

        public string Title { get; set; }

        public int Order { get; set; }

        public static ImageBlockDTO FromImageBlock(ImageBlock imageBlock) =>
            imageBlock is null
                ? null
                : new ImageBlockDTO {
                    ID = imageBlock.ID,
                    Page = imageBlock.Page,
                    Title = imageBlock.Title,
                    Order = imageBlock.Order
                };
    }
}
