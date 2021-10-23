using System;
using GTDPad.Domain;

namespace GTDPad.DTO
{
    public class RichTextBlockDTO : DTOBase<RichTextBlockDTO>
    {
        public Guid Page { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public static RichTextBlockDTO FromRichTextBlock(RichTextBlock richTextBlock) =>
            richTextBlock is null
                ? null
                : new RichTextBlockDTO {
                    ID = richTextBlock.ID,
                    Page = richTextBlock.Page,
                    Title = richTextBlock.Title,
                    Order = richTextBlock.Order
                };
    }
}
