namespace Banks.AccountInterfaces
{
    public interface IAccrualAccount
    {
        void AccrueFunds(decimal amount);
    }
}