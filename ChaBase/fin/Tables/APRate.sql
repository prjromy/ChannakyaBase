CREATE TABLE [fin].[APRate] (
    [IAccno] INT  NOT NULL,
    [PPRate] REAL NOT NULL,
    [PIRate] REAL NOT NULL,
    CONSTRAINT [PK_APRate] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_AccountPRate_AccountDetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

