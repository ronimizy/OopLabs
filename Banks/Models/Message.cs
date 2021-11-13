using Microsoft.EntityFrameworkCore;
using Utility.Extensions;

namespace Banks.Models
{
    [Owned]
    public class Message
    {
        public Message(string title, string body)
        {
            Title = title.ThrowIfNull(nameof(title));
            Body = body.ThrowIfNull(nameof(body));
        }

#pragma warning disable 8618
        private Message() { }
#pragma warning restore 8618

        public string Title { get; private init; }
        public string Body { get; private init; }
    }
}