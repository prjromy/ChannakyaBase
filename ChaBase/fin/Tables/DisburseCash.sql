CREATE TABLE [fin].[DisburseCash] (
    [DisCashId]   INT    IDENTITY (1, 1) NOT NULL,
    [DisburseId]  INT    NOT NULL,
    [Amount]      MONEY  NULL,
    [RecieveFrom] INT    NULL,
    [TNo]         BIGINT NULL,
    CONSTRAINT [PK_DisCash] PRIMARY KEY CLUSTERED ([DisCashId] ASC),
    CONSTRAINT [FK_DisCash_DisbursementMain] FOREIGN KEY ([DisburseId]) REFERENCES [fin].[DisbursementMain] ([DisburseId])
);

