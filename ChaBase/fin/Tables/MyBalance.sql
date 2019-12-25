CREATE TABLE [fin].[MyBalance] (
    [BId]    INT   IDENTITY (1, 1) NOT NULL,
    [UserID] INT   NOT NULL,
    [Amount] MONEY NULL,
    [Currid] INT   NOT NULL,
    CONSTRAINT [PK_MyBalance] PRIMARY KEY CLUSTERED ([BId] ASC)
);

