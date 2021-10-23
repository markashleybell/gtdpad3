using System;
using System.Collections.Generic;
using System.Linq;

namespace GTDPad.Domain
{
    public class List : Section
    {
        public List(
            Guid id,
            Guid page,
            Guid owner,
            string title,
            int order)
            : this(
                id,
                page,
                owner,
                title,
                order,
                default)
        { }

        public List(
            Guid id,
            Guid page,
            Guid owner,
            string title,
            int order,
            IEnumerable<ListItem> items)
            : base(
                id,
                page,
                owner,
                title,
                order) =>
            Items = items ?? Enumerable.Empty<ListItem>();

        public IEnumerable<ListItem> Items { get; }

        public List With(string title = default, int? order = default) =>
            new List(ID, Page, Owner, title ?? Title, order ?? Order);
    }
}
