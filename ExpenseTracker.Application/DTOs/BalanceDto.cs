namespace ExpenseTracker.Application.DTOs
{
    public class BalanceDto
    {
        public int UserId { get; init; }
        public decimal TotalIncomes { get; init; }
        public decimal TotalExpenses { get; init; }
        public decimal Balance { get; init; }
    }
}
