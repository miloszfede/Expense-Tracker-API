namespace ExpenseTracker.Infrastructure.Queries
{
    public static class CategoryQueries
    {
        public static readonly string GetById = BaseQueries.GetCategoryById();
        
        public static readonly string GetAll = BaseQueries.GetAllCategories();
        
        public static readonly string GetByUserId = BaseQueries.GetCategoriesByUserId();
        
        public static readonly string GetByType = BaseQueries.GetCategoriesByType();
        
        public static readonly string GetByUserIdNameAndType = BaseQueries.GetCategoryByUserIdNameAndType();
        
        public static readonly string Insert = BaseQueries.InsertCategory();
        
        public static readonly string Update = BaseQueries.UpdateCategory();
        
        public static readonly string Delete = BaseQueries.DeleteByInternalId("Categories");
    }
}
