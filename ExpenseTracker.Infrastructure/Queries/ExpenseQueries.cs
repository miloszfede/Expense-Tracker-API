namespace ExpenseTracker.Infrastructure.Queries
{
    public static class ExpenseQueries
    {
        public static readonly string GetById = BaseQueries.GetTransactionEntityById("e", "Expenses");
        
        public static readonly string GetAll = BaseQueries.GetAllTransactionEntities("e", "Expenses");
        
        public static readonly string GetByUserId = BaseQueries.GetTransactionEntitiesByUserId("e", "Expenses");
        
        public static readonly string GetByCategoryId = BaseQueries.GetTransactionEntitiesByCategoryId("e", "Expenses");
        
        public static readonly string GetByDateRange = BaseQueries.GetTransactionEntitiesByDateRange("e", "Expenses");
        
        public static readonly string Insert = BaseQueries.InsertTransactionEntity("Expenses");
        
        public static readonly string Update = BaseQueries.UpdateTransactionEntity("Expenses");
        
        public static readonly string Delete = BaseQueries.DeleteByInternalId("Expenses");
    }
}
