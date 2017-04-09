CREATE TABLE [dbo].[Transactions] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [Price]      DECIMAL (19, 4) NOT NULL,
    [DateTime]   DATETIME2 (7)   NOT NULL,
    [CustomerID] INT             NOT NULL,
    [EmployeeID] INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Transactions_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([ID]),
    CONSTRAINT [FK_Transactions_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([ID])
);

