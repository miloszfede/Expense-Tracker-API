namespace ExpenseTracker.Infrastructure.Queries
{
    public static class IncomeQueries
    {
        public static readonly string GetById = BaseQueries.GetTransactionEntityById("i", "Incomes");
        
        public static readonly string GetAll = BaseQueries.GetAllTransactionEntities("i", "Incomes");
        
        public static readonly string GetByUserId = BaseQueries.GetTransactionEntitiesByUserId("i", "Incomes");
        
        public static readonly string GetByCategoryId = BaseQueries.GetTransactionEntitiesByCategoryId("i", "Incomes");
        
        public static readonly string GetByDateRange = BaseQueries.GetTransactionEntitiesByDateRange("i", "Incomes");
        
        public static readonly string Insert = BaseQueries.InsertTransactionEntity("Incomes");
        
        public static readonly string Update = BaseQueries.UpdateTransactionEntity("Incomes");
        
        public static readonly string Delete = BaseQueries.DeleteByInternalId("Incomes");
    }
}
