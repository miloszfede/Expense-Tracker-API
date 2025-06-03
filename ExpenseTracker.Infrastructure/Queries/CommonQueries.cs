namespace ExpenseTracker.Infrastructure.Queries
{
    public static class CommonQueries
    {
        public const string GetUserExternalIdByInternalId = @"
            SELECT Id as ExternalId
            FROM Users 
            WHERE InternalId = @UserId";

        public const string GetCategoryExternalIdByInternalId = @"
            SELECT Id as ExternalId
            FROM Categories 
            WHERE InternalId = @CategoryId";

        public const string GetUserGuidForQuery = @"
            SELECT Id FROM Users WHERE InternalId = @UserId";

        public const string GetCategoryGuidForQuery = @"
            SELECT Id FROM Categories WHERE InternalId = @CategoryId";
    }
}
