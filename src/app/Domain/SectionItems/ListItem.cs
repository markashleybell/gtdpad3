using System;

namespace GTDPad.Domain
{
    public class ListItem
    {
        public ListItem(
            Guid id,
            string text,
            int order)
        {
            ID = id;
            Text = text;
            Order = order;
        }

        public Guid ID { get; }

        public string Text { get; }

        public int Order { get; }

        public ListItem With(string text = default, int? order = default) =>
            new ListItem(ID, text ?? Text, order ?? Order);
    }
}
