CREATE TABLE [fin].[DisburseCheque] (
    [DisChequeId] INT           IDENTITY (1, 1) NOT NULL,
    [DisburseId]  INT           NOT NULL,
    [ChequeNo]    VARCHAR (100) NOT NULL,
    [BankId]      INT           NOT NULL,
    [Amount]      MONEY         NULL,
    CONSTRAINT [PK_DisburseCheque] PRIMARY KEY CLUSTERED ([DisChequeId] ASC),
    CONSTRAINT [FK_DisburseCheque_DisbursementMain] FOREIGN KEY ([DisburseId]) REFERENCES [fin].[DisbursementMain] ([DisburseId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cheque Amount', @level0type = N'SCHEMA', @level0name = N'fin', @level1type = N'TABLE', @level1name = N'DisburseCheque', @level2type = N'COLUMN', @level2name = N'Amount';

