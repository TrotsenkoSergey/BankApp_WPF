namespace BankApp
{
    /// <summary>
    /// Provides an opportunity to place a deposit.
    /// </summary>
    public interface IDeposit
    {
        /// <summary>
        /// With draw.
        /// </summary>
        /// <param name="concreteDeposit"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Customer WithDrawDeposit(object concreteDeposit, decimal amount);

        /// <summary>
        /// Place new deposit.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Customer AddNewDeposit(decimal amount);

        /// <summary>
        /// Replenish the deposit.
        /// </summary>
        /// <param name="concreteDeposit"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Customer AddAmountExistingDeposit(object concreteDeposit, decimal amount);
    }
}
