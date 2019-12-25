CREATE TABLE [fin].[IchkBounce] (
    [IAccno]   INT            NOT NULL,
    [Chkno]    NUMERIC (18)   NOT NULL,
    [Rmks]     NVARCHAR (500) NULL,
    [TDate]    DATETIME       NOT NULL,
    [PostedBy] INT            NOT NULL,
    [Postedon] DATETIME       NOT NULL,
    CONSTRAINT [PK_IchkBounce] PRIMARY KEY CLUSTERED ([Chkno] ASC),
    CONSTRAINT [FK_IchkBounce_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

