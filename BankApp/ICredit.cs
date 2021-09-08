namespace BankApp
{
    /// <summary>
    /// Provides an opportunity to receive and repay a loan.
    /// </summary>
    public interface ICredit
    {
        /// <summary>
        /// Get credit.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Customer GetCredit(decimal amount);

        /// <summary>
        /// Repay loan.
        /// </summary>
        /// <param name="concreteCredit"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Customer RepayLoan(object concreteCredit, decimal amount);
    }
}