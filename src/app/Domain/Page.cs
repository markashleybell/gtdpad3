using System;
using System.Collections.Generic;
using System.Linq;
using GTDPad.Support;

namespace GTDPad.Domain
{
    public class Page
    {
        public Page(
            Guid id,
            Guid owner,
            string title,
            string slug,
            int order)
            : this(
                  id,
                  owner,
                  title,
                  slug,
                  order,
                  default)
        {
        }

        public Page(
            Guid id,
            Guid owner,
            string title,
            string slug,
            int order,
            IEnumerable<Section> sections)
        {
            Guard.AgainstEmpty(id, nameof(id));
            Guard.AgainstEmpty(owner, nameof(owner));
            Guard.AgainstNullOrEmpty(title, nameof(title));
            Guard.AgainstNullOrEmpty(slug, nameof(slug));

            ID = id;
            Owner = owner;
            Title = title;
            Slug = slug;
            Order = order;
            Sections = sections ?? Enumerable.Empty<Section>();
        }

        public Guid ID { get; }

        public Guid Owner { get; }

        public string Title { get; }

        public string Slug { get; }

        public int Order { get; }

        public IEnumerable<Section> Sections { get; }

        public Page With(string title = default, string slug = default, int? order = default) =>
            new Page(ID, Owner, title ?? Title, slug ?? Slug, order ?? Order);
    }
}
