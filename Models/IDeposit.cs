namespace BankApp_WPF.Models
{

    public interface IDeposit
    {

        Customer WithDraw(object concreteDeposit, decimal amount);

        Customer AddNewDeposit(decimal amount);

        Customer AddAmountExistingDeposit(object concreteDeposit, decimal amount);
    }
}
