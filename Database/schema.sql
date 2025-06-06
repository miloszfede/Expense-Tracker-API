SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
GO
SET NOCOUNT ON;
GO

CREATE TABLE [Users] (
  [InternalId] INT PRIMARY KEY IDENTITY(1, 1),
  [Id] UNIQUEIDENTIFIER UNIQUE NOT NULL DEFAULT (NEWID()),
  [Username] NVARCHAR(50) UNIQUE NOT NULL,
  [Email] VARCHAR(100) UNIQUE NOT NULL,
  [PasswordHash] NVARCHAR(MAX) NOT NULL,
  [RoleId] INT NOT NULL,
  [CreatedAt] DATETIME NOT NULL DEFAULT (GETDATE()),
  [UpdatedAt] DATETIME NOT NULL DEFAULT (GETDATE())
) WITH (DATA_COMPRESSION = PAGE);
GO

CREATE TABLE [Roles] (
  [InternalId] INT PRIMARY KEY IDENTITY(1, 1),
  [Id] UNIQUEIDENTIFIER UNIQUE NOT NULL DEFAULT (NEWID()),
  [Name] NVARCHAR(50) UNIQUE NOT NULL,
  [Description] NVARCHAR(200) NOT NULL
) WITH (DATA_COMPRESSION = PAGE);
GO

CREATE TABLE [Categories] (
  [InternalId] INT PRIMARY KEY IDENTITY(1, 1),
  [Id] UNIQUEIDENTIFIER UNIQUE NOT NULL DEFAULT (NEWID()),
  [UserId] UNIQUEIDENTIFIER,
  [Name] NVARCHAR(100) NOT NULL,
  [Type] NVARCHAR(20) NOT NULL,
  [IsDefault] BIT NOT NULL DEFAULT (0)
) WITH (DATA_COMPRESSION = PAGE);
GO

CREATE TABLE [Incomes] (
  [InternalId] INT PRIMARY KEY IDENTITY(1, 1),
  [Id] UNIQUEIDENTIFIER UNIQUE NOT NULL DEFAULT (NEWID()),
  [UserId] UNIQUEIDENTIFIER NOT NULL,
  [Amount] DECIMAL(18,2) NOT NULL,
  [Date] DATETIME NOT NULL DEFAULT (GETDATE()),
  [Note] NVARCHAR(500),
  [CategoryId] UNIQUEIDENTIFIER NOT NULL
) WITH (DATA_COMPRESSION = PAGE);
GO

CREATE TABLE [Expenses] (
  [InternalId] INT PRIMARY KEY IDENTITY(1, 1),
  [Id] UNIQUEIDENTIFIER UNIQUE NOT NULL DEFAULT (NEWID()),
  [UserId] UNIQUEIDENTIFIER NOT NULL,
  [Amount] DECIMAL(18,2) NOT NULL,
  [Date] DATETIME NOT NULL DEFAULT (GETDATE()),
  [Note] NVARCHAR(500),
  [CategoryId] UNIQUEIDENTIFIER NOT NULL
) WITH (DATA_COMPRESSION = PAGE);
GO

CREATE UNIQUE INDEX [Users_index_0] ON [Users] ("Username");
GO

CREATE UNIQUE INDEX [Users_index_1] ON [Users] ("Email");
GO

CREATE UNIQUE INDEX [Users_index_2] ON [Users] ("Id");
GO

CREATE UNIQUE INDEX [Categories_index_3] ON [Categories] ("UserId", "Name", "Type");
GO

CREATE INDEX [Categories_index_4] ON [Categories] ("Type", "IsDefault");
GO

CREATE UNIQUE INDEX [Categories_index_5] ON [Categories] ("Id");
GO

CREATE INDEX [Incomes_index_6] ON [Incomes] ("UserId", "Date");
GO

CREATE INDEX [Incomes_index_7] ON [Incomes] ("Date");
GO

CREATE INDEX [Incomes_index_8] ON [Incomes] ("CategoryId");
GO

CREATE UNIQUE INDEX [Incomes_index_9] ON [Incomes] ("Id");
GO

CREATE INDEX [Expenses_index_10] ON [Expenses] ("UserId", "Date");
GO

CREATE INDEX [Expenses_index_11] ON [Expenses] ("Date");
GO

CREATE INDEX [Expenses_index_12] ON [Expenses] ("CategoryId");
GO

CREATE UNIQUE INDEX [Expenses_index_13] ON [Expenses] ("Id");
GO

CREATE UNIQUE INDEX [Roles_index_14] ON [Roles] ("Name");
GO

CREATE UNIQUE INDEX [Roles_index_15] ON [Roles] ("Id");
GO

ALTER TABLE [Users] ADD FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([InternalId]) ON DELETE NO ACTION;
GO

ALTER TABLE [Incomes] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [Expenses] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [Categories] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [Incomes] ADD FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION;
GO

ALTER TABLE [Expenses] ADD FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [dbo].[Roles] ([Id], [Name], [Description]) VALUES
(NEWID(), 'Admin', 'Administrator with full access to the system'),
(NEWID(), 'User', 'Regular user with access to their own data');
GO

-- Default admin user (username: admin, password: P4sword!@)
INSERT INTO [dbo].[Users] ([Id], [Username], [Email], [PasswordHash], [RoleId])
SELECT NEWID(), 'admin', 'admin@expensetracker.local', 'D596iywVyn4WZax6tbEncbQvs184mREFuzLNyZGBfe97w5S6tTNtyhm40k/Jqt4j', [InternalId]
FROM [dbo].[Roles] WHERE [Name] = 'Admin';
GO

