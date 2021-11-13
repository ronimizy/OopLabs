using System;
using Utility.Extensions;

namespace Banks.Models
{
    public class EmailPreview
    {
        public EmailPreview(Guid id, string sender, string title, bool viewed)
        {
            Id = id;
            Sender = sender.ThrowIfNull(nameof(sender));
            Title = title.ThrowIfNull(nameof(title));
            Viewed = viewed;
        }

        public Guid Id { get; private init; }
        public string Sender { get; private init; }
        public string Title { get; private init; }
        public bool Viewed { get; private init; }
    }
}