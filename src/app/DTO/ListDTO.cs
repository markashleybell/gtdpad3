using System;
using GTDPad.Domain;

namespace GTDPad.DTO
{
    public class ListDTO : DTOBase<ListDTO>
    {
        public Guid Page { get; set; }

        public string Title { get; set; }

        public int Order { get; set; }

        public static ListDTO FromList(List list) =>
            list is null
                ? null
                : new ListDTO {
                    ID = list.ID,
                    Page = list.Page,
                    Title = list.Title,
                    Order = list.Order
                };
    }
}
