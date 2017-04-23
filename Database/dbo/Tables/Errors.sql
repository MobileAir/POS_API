CREATE TABLE [dbo].[Errors] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [Message]      NVARCHAR (500) NULL,
    [Controller]   NVARCHAR (50)  NULL,
    [UserAgent]    NVARCHAR (500) NULL,
    [StackTrace]   NVARCHAR (MAX) NULL,
    [Ip]           NVARCHAR (100) NULL,
    [TargetResult] NVARCHAR (200) NULL,
    [DateTime]     DATETIME2 (7)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

