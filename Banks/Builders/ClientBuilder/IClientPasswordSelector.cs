namespace Banks.Builders.ClientBuilder
{
    public interface IClientPasswordSelector
    {
        IClientEmailAddressSelector WithPassword(string password);
    }
}