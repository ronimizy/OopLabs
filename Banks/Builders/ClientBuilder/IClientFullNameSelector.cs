namespace Banks.Builders.ClientBuilder
{
    public interface IClientFullNameSelector
    {
        IClientPasswordSelector Called(string name, string surname);
    }
}