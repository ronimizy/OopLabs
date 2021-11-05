namespace Banks.Builders.CreditAccountPlanBuilder
{
    public interface ICreditLimitSelector
    {
        ICreditPercentageSelector LimitedTo(decimal limit);
    }
}