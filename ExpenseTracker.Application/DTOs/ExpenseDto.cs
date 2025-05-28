using System;

namespace ExpenseTracker.Application.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    
    public class CreateExpenseDto
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int CategoryId { get; set; }
    }
    
    public class UpdateExpenseDto
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int CategoryId { get; set; }
    }
}
