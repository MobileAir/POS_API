CREATE TABLE [dbo].[Payments] (
    [ID]              INT             IDENTITY (1, 1) NOT NULL,
    [Amount]          DECIMAL (19, 4) NOT NULL,
    [DateTime]        DATETIME2 (7)   NOT NULL,
    [PaymentMethodID] INT             NOT NULL,
    [TransactionID]   INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Payments_PaymentMethodS] FOREIGN KEY ([PaymentMethodID]) REFERENCES [dbo].[PaymentMethods] ([ID]),
    CONSTRAINT [FK_Payments_TransactionS] FOREIGN KEY ([TransactionID]) REFERENCES [dbo].[Transactions] ([ID])
);

