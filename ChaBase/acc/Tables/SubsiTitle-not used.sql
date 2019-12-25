CREATE TABLE [acc].[SubsiTitle-not used] (
    [STid]       INT          IDENTITY (1, 1) NOT NULL,
    [FId]        INT          NOT NULL,
    [STitleName] VARCHAR (50) NOT NULL,
    [STPrefix]   VARCHAR (20) NOT NULL,
    [Slength]    INT          NOT NULL,
    [CurrentNo]  INT          NOT NULL,
    CONSTRAINT [PK_SubsiTitle] PRIMARY KEY CLUSTERED ([STid] ASC),
    CONSTRAINT [FK_SubsiTitle_FinAcc] FOREIGN KEY ([FId]) REFERENCES [acc].[FinAcc] ([Fid])
);

