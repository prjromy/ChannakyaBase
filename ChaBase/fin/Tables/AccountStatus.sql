CREATE TABLE [fin].[AccountStatus] (
    [AsId]         TINYINT      IDENTITY (1, 1) NOT NULL,
    [AccountState] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AccountStatus] PRIMARY KEY CLUSTERED ([AsId] ASC)
);

