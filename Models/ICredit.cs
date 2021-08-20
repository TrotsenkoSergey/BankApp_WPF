namespace BankApp_WPF.Models
{
    public interface ICredit
    {
        void GetCredit(decimal amount);

        void RepayLoan(object concreteCredit, decimal amount);
    }
}