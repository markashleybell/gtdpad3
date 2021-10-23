using System;
using System.Collections.Generic;
using System.Linq;

namespace GTDPad.Domain
{
    public class ImageBlock : Section
    {
        public ImageBlock(
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

        public ImageBlock(
            Guid id,
            Guid page,
            Guid owner,
            string title,
            int order,
            IEnumerable<Image> images)
            : base(
                id,
                page,
                owner,
                title,
                order) =>
            Images = images ?? Enumerable.Empty<Image>();

        public IEnumerable<Image> Images { get; }

        public ImageBlock With(string title = default, int? order = default) =>
            new ImageBlock(ID, Page, Owner, title ?? Title, order ?? Order);
    }
}
