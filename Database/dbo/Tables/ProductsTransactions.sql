CREATE TABLE [dbo].[ProductsTransactions] (
    [ID]            INT IDENTITY (1, 1) NOT NULL,
    [ProductID]     INT NOT NULL,
    [TransactionID] INT NOT NULL,
    CONSTRAINT [PK_ProductsTransactions_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductsTransactions_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProductsTransactions_Transactions] FOREIGN KEY ([TransactionID]) REFERENCES [dbo].[Transactions] ([ID]) ON DELETE CASCADE
);

