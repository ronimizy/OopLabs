using Banks.Entities;
using Banks.Models;
using Banks.Tools;

namespace Banks.Builders.ClientBuilder
{
    public interface IClientOptionalInfoSelector : IBuilder<Client>
    {
        IBuilder<Client> Builder { get; }

        IClientOptionalInfoSelector WithAddress(string? address);
        IClientOptionalInfoSelector WithPassportData(PassportData? passportData);
    }
}