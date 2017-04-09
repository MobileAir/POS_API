CREATE TABLE [dbo].[Users] (
    [UserId]         INT            IDENTITY (1, 1) NOT NULL,
    [Username]       NVARCHAR (50)  NOT NULL,
    [Salt]           NVARCHAR (64)  NULL,
    [Password]       NVARCHAR (128) NOT NULL,
    [Name]           NVARCHAR (50)  NULL,
    [RequestAllowed] INT            CONSTRAINT [DF_User_RequestAllowed] DEFAULT ((10)) NOT NULL,
    [IsSuperUser]    BIT            CONSTRAINT [DF_User_IsSuperUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

