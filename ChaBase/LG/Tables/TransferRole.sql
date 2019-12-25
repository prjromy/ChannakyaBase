CREATE TABLE [LG].[TransferRole] (
    [TURId]   INT           IDENTITY (1, 1) NOT NULL,
    [MUid]    INT           NOT NULL,
    [TUid]    INT           NOT NULL,
    [RoleId]  INT           NOT NULL,
    [EffFrom] DATETIME      NULL,
    [EffTo]   DATETIME      NULL,
    [Status]  BIT           NULL,
    [Remarks] VARCHAR (500) NULL,
    CONSTRAINT [PK_TransferRole] PRIMARY KEY CLUSTERED ([TURId] ASC)
);

