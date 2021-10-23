using GTDPad.Domain;

namespace GTDPad.DTO
{
    public class PageDTO : DTOBase<PageDTO>
    {
        public string Title { get; set; }

        public string Slug { get; set; }

        public int Order { get; set; }

        public static PageDTO FromPage(Page page) =>
            page is null
                ? null
                : new PageDTO {
                    ID = page.ID,
                    Title = page.Title,
                    Slug = page.Slug,
                    Order = page.Order
                };
    }
}
