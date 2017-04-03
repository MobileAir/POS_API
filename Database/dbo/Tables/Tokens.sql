CREATE TABLE [dbo].[Tokens] (
    [TokenId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserId]    INT            NOT NULL,
    [AuthToken] NVARCHAR (250) NOT NULL,
    [IssuedOn]  DATETIME2 (7)  NOT NULL,
    [ExpiresOn] DATETIME2 (7)  NOT NULL,
    [Request]   INT            CONSTRAINT [DF_Tokens_Requests] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED ([TokenId] ASC),
    CONSTRAINT [FK_Tokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);

