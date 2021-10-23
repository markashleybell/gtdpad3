using System;

namespace GTDPad.Domain
{
    public class RichTextBlock : Section
    {
        public RichTextBlock(
            Guid id,
            Guid page,
            Guid owner,
            string title,
            string text,
            int order)
        : base(
              id,
              page,
              owner,
              title,
              order) =>
            Text = text;

        public string Text { get; }

        public RichTextBlock With(string title = default, string text = default, int? order = default) =>
            new RichTextBlock(ID, Page, Owner, title ?? Title, text ?? Text, order ?? Order);
    }
}
