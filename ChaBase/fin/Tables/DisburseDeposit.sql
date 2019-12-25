CREATE TABLE [fin].[DisburseDeposit] (
    [DisDepositId]  INT   IDENTITY (1, 1) NOT NULL,
    [DisburseId]    INT   NOT NULL,
    [DepositIaccno] INT   NOT NULL,
    [Amount]        MONEY NULL,
    CONSTRAINT [PK_DisburseDeposit] PRIMARY KEY CLUSTERED ([DisDepositId] ASC),
    CONSTRAINT [FK_DisburseDeposit_ADetail] FOREIGN KEY ([DepositIaccno]) REFERENCES [fin].[ADetail] ([IAccno]),
    CONSTRAINT [FK_DisburseDeposit_DisbursementMain] FOREIGN KEY ([DisburseId]) REFERENCES [fin].[DisbursementMain] ([DisburseId])
);

