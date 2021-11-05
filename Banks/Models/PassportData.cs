using System;
using System.ComponentModel.DataAnnotations;
using Utility.Extensions;

namespace Banks.Models
{
    public class PassportData
    {
        public PassportData(string serial, string number)
        {
            Serial = serial.ThrowIfNull(nameof(serial));
            Number = number.ThrowIfNull(nameof(number));
        }

#pragma warning disable 8618
        private PassportData() { }
#pragma warning restore 8618

        [Key]
        public Guid? Id { get; private init; }

        public string Serial { get; private init; }
        public string Number { get; private init; }
    }
}