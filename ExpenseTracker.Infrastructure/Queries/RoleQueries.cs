namespace ExpenseTracker.Infrastructure.Queries
{
    public static class RoleQueries
    {
        public static readonly string GetById = BaseQueries.GetRoleById();
        
        public static readonly string GetAll = BaseQueries.GetAllRoles();
        
        public static readonly string GetByName = BaseQueries.GetRoleByName();
        
        public static readonly string Insert = BaseQueries.InsertRole();
        
        public static readonly string Update = BaseQueries.UpdateRole();
        
        public static readonly string Delete = BaseQueries.DeleteByInternalId("Roles");
    }
}
