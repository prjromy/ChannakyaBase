CREATE TABLE [fin].[APolicyInterest] (
    [IAccno] INT NOT NULL,
    [PSID]   INT NOT NULL,
    CONSTRAINT [PK_APolInt] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_AccountCreInfo_AccountDetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

