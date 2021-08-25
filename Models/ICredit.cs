namespace BankApp_WPF.Models
{

    public interface ICredit
    {

        Customer GetCredit(decimal amount);

        Customer RepayLoan(object concreteCredit, decimal amount);
    }
}