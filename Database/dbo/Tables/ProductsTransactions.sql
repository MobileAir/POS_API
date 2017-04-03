CREATE TABLE [dbo].[ProductsTransactions] (
    [ProductID]     INT NOT NULL,
    [TransactionID] INT NOT NULL,
    CONSTRAINT [FK_ProductsTransactions_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ID]),
    CONSTRAINT [FK_ProductsTransactions_Transactions] FOREIGN KEY ([TransactionID]) REFERENCES [dbo].[Transactions] ([ID])
);

