namespace BankApp_WPF.Models
{
    public interface IDeposit
    {

        void WithDraw(object concreteDeposit, decimal amount);

        void AddNewDeposit(decimal amount);

        void AddAmountExistingDeposit(object concreteDeposit, decimal amount);
    }
}
