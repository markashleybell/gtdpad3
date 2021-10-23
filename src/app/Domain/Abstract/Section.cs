using System;
using GTDPad.Support;

namespace GTDPad.Domain
{
    public abstract class Section
    {
        protected Section(
            Guid id,
            Guid page,
            Guid owner,
            string title,
            int order)
        {
            Guard.AgainstEmpty(id, nameof(id));
            Guard.AgainstEmpty(page, nameof(page));
            Guard.AgainstEmpty(owner, nameof(owner));
            Guard.AgainstNullOrEmpty(title, nameof(title));

            ID = id;
            Page = page;
            Owner = owner;
            Title = title;
            Order = order;
        }

        public Guid ID { get; }

        public Guid Page { get; }

        public Guid Owner { get; }

        public string Title { get; }

        public int Order { get; }
    }
}
