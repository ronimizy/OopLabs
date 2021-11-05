using Banks.Models;

namespace Banks.Builders.ClientBuilder
{
    public interface IClientEmailAddressSelector
    {
        IClientOptionalInfoSelector WithEmailAddress(EmailAddress emailAddress);
    }
}