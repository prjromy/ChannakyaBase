CREATE TABLE [fin].[Duration] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Duration] VARCHAR (50) NOT NULL,
    [Value]    FLOAT (53)   NOT NULL,
    CONSTRAINT [PK_DepositValue] PRIMARY KEY CLUSTERED ([Id] ASC)
);

