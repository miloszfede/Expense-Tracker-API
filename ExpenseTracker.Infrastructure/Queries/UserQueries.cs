namespace ExpenseTracker.Infrastructure.Queries
{
    public static class UserQueries
    {
        public static readonly string GetById = BaseQueries.GetUserById();
        
        public static readonly string GetAll = BaseQueries.GetAllUsers();
        
        public static readonly string GetByEmail = BaseQueries.GetUserByEmail();
        
        public static readonly string GetByUsername = BaseQueries.GetUserByUsername();
        
        public static readonly string Insert = BaseQueries.InsertUser();
        
        public static readonly string Update = BaseQueries.UpdateUser();
        
        public static readonly string Delete = BaseQueries.DeleteByInternalId("Users");
    }
}
