using System;
using System.ComponentModel.DataAnnotations;
using Utility.Extensions;

namespace Banks.Models
{
    public class Info
    {
        public Info(string name, string description)
        {
            Title = name.ThrowIfNull(nameof(name));
            Description = description.ThrowIfNull(nameof(name));
        }

#pragma warning disable 8618
        private Info() { }
#pragma warning restore 8618

        [Key]
        public Guid? Id { get; private init; }

        public string Title { get; private init; }
        public string Description { get; private init; }

        public override string ToString()
            => $"{Title} - {Description}";
    }
}