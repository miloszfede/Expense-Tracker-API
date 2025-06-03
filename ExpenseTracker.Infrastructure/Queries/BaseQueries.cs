namespace ExpenseTracker.Infrastructure.Queries
{
    public static class BaseQueries
    {
        #region Generic Transaction Entity Queries (Income/Expense)
        
        public const string TransactionEntitySelectBase = @"
            SELECT {0}.InternalId as Id, {0}.Id as ExternalId, 
                   (SELECT InternalId FROM Users WHERE Id = {0}.UserId) as UserId,
                   {0}.Amount, {0}.Date, {0}.Note, 
                   (SELECT InternalId FROM Categories WHERE Id = {0}.CategoryId) as CategoryId,
                   c.Name as CategoryName
            FROM {1} {0}
            LEFT JOIN Categories c ON {0}.CategoryId = c.Id";

        public static string GetTransactionEntityById(string tableAlias, string tableName) =>
            $"{TransactionEntitySelectBase.Replace("{0}", tableAlias).Replace("{1}", tableName)}" +
            $"\n            WHERE {tableAlias}.InternalId = @Id";

        public static string GetAllTransactionEntities(string tableAlias, string tableName) =>
            $"{TransactionEntitySelectBase.Replace("{0}", tableAlias).Replace("{1}", tableName)}" +
            $"\n            ORDER BY {tableAlias}.Date DESC";

        public static string GetTransactionEntitiesByUserId(string tableAlias, string tableName) =>
            $"{TransactionEntitySelectBase.Replace("{0}", tableAlias).Replace("{1}", tableName)}" +
            $"\n            WHERE {tableAlias}.UserId = (SELECT Id FROM Users WHERE InternalId = @UserId)" +
            $"\n            ORDER BY {tableAlias}.Date DESC";

        public static string GetTransactionEntitiesByCategoryId(string tableAlias, string tableName) =>
            $"{TransactionEntitySelectBase.Replace("{0}", tableAlias).Replace("{1}", tableName)}" +
            $"\n            WHERE {tableAlias}.CategoryId = (SELECT Id FROM Categories WHERE InternalId = @CategoryId)" +
            $"\n            ORDER BY {tableAlias}.Date DESC";

        public static string GetTransactionEntitiesByDateRange(string tableAlias, string tableName) =>
            $"{TransactionEntitySelectBase.Replace("{0}", tableAlias).Replace("{1}", tableName)}" +
            $"\n            WHERE {tableAlias}.UserId = (SELECT Id FROM Users WHERE InternalId = @UserId)" +
            $"\n            AND {tableAlias}.Date >= @StartDate" +
            $"\n            AND {tableAlias}.Date <= @EndDate" +
            $"\n            ORDER BY {tableAlias}.Date DESC";

        #endregion

        #region Generic CRUD Operations

        public static string InsertTransactionEntity(string tableName) =>
            $@"INSERT INTO {tableName} (Id, UserId, Amount, Date, Note, CategoryId)
            OUTPUT INSERTED.InternalId
            VALUES (@ExternalId, 
                    (SELECT Id FROM Users WHERE InternalId = @UserId), 
                    @Amount, @Date, @Note, 
                    (SELECT Id FROM Categories WHERE InternalId = @CategoryId))";

        public static string UpdateTransactionEntity(string tableName) =>
            $@"UPDATE {tableName} 
            SET Amount = @Amount, 
                Date = @Date, 
                Note = @Note, 
                CategoryId = (SELECT Id FROM Categories WHERE InternalId = @CategoryId)
            WHERE InternalId = @Id";

        public static string DeleteByInternalId(string tableName) =>
            $"DELETE FROM {tableName} WHERE InternalId = @Id";

        #endregion

        #region Generic Simple Entity Queries

        public static string GetSimpleEntityById(string selectClause, string tableName, string whereClause = "InternalId = @Id") =>
            $@"SELECT {selectClause} 
            FROM {tableName} 
            WHERE {whereClause}";

        public static string GetAllSimpleEntities(string selectClause, string tableName, string orderByClause = "") =>
            $@"SELECT {selectClause} 
            FROM {tableName}" +
            (string.IsNullOrEmpty(orderByClause) ? "" : $"\n            ORDER BY {orderByClause}");

        #endregion

        #region User Entity Specific Templates

        private const string UserSelectClause = "InternalId as Id, Id as ExternalId, Username, Email, PasswordHash, CreatedAt, UpdatedAt";

        public static string GetUserById() =>
            GetSimpleEntityById(UserSelectClause, "Users");

        public static string GetAllUsers() =>
            GetAllSimpleEntities(UserSelectClause, "Users", "CreatedAt DESC");

        public static string GetUserByEmail() =>
            GetSimpleEntityById(UserSelectClause, "Users", "Email = @Email");

        public static string GetUserByUsername() =>
            GetSimpleEntityById(UserSelectClause, "Users", "Username = @Username");

        public static string InsertUser() =>
            @"INSERT INTO Users (Id, Username, Email, PasswordHash, CreatedAt, UpdatedAt)
            OUTPUT INSERTED.InternalId
            VALUES (@ExternalId, @Username, @Email, @PasswordHash, @CreatedAt, @UpdatedAt)";

        public static string UpdateUser() =>
            @"UPDATE Users 
            SET Username = @Username, 
                Email = @Email, 
                PasswordHash = @PasswordHash, 
                UpdatedAt = @UpdatedAt
            WHERE InternalId = @Id";

        #endregion

        #region Category Entity Specific Templates

        private const string CategorySelectClause = "c.InternalId as Id, c.Id as ExternalId, c.Name, c.Type, c.IsDefault, (SELECT InternalId FROM Users WHERE Id = c.UserId) as UserId";

        public static string GetCategoryById() =>
            $@"SELECT {CategorySelectClause}
            FROM Categories c
            WHERE c.InternalId = @Id";

        public static string GetAllCategories() =>
            $@"SELECT {CategorySelectClause}
            FROM Categories c
            ORDER BY c.Name";

        public static string GetCategoriesByUserId() =>
            $@"SELECT {CategorySelectClause}
            FROM Categories c
            WHERE c.UserId = (SELECT Id FROM Users WHERE InternalId = @UserId) OR c.IsDefault = 1
            ORDER BY c.IsDefault DESC, c.Name";

        public static string GetCategoriesByType() =>
            $@"SELECT {CategorySelectClause}
            FROM Categories c
            WHERE c.Type = @Type
            ORDER BY c.IsDefault DESC, c.Name";

        public static string GetCategoryByUserIdNameAndType() =>
            $@"SELECT {CategorySelectClause}
            FROM Categories c
            WHERE c.UserId = (SELECT Id FROM Users WHERE InternalId = @UserId) 
            AND c.Name = @Name 
            AND c.Type = @Type";

        public static string InsertCategory() =>
            @"INSERT INTO Categories (Id, UserId, Name, Type, IsDefault)
            OUTPUT INSERTED.InternalId
            VALUES (@ExternalId, (SELECT Id FROM Users WHERE InternalId = @UserId), @Name, @Type, @IsDefault)";

        public static string UpdateCategory() =>
            @"UPDATE Categories 
            SET Name = @Name, 
                Type = @Type, 
                IsDefault = @IsDefault
            WHERE InternalId = @Id";

        #endregion
    }
}
