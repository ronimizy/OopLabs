using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Extensions;

namespace Banks.Models
{
    public class Email
    {
        public Email(EmailAddress receiver, string sender, Message message)
        {
            Receiver = receiver.ThrowIfNull(nameof(receiver));
            Sender = sender.ThrowIfNull(nameof(sender));
            Message = message.ThrowIfNull(nameof(message));
        }

#pragma warning disable 8618
        private Email() { }
#pragma warning restore 8618

        [Key]
        public Guid? Id { get; private init; }

        public EmailAddress Receiver { get; private init; }
        public string Sender { get; private init; }
        public Message Message { get; private init; }
        public bool Viewed { get; internal set; }

        [NotMapped]
        public EmailPreview Preview => new EmailPreview(Id.ThrowIfNull(nameof(Id)), Sender, Message.Title, Viewed);
    }
}