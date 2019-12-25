CREATE TABLE [fin].[DisburseCharge] (
    [DisChargeId] INT           IDENTITY (1, 1) NOT NULL,
    [DisburseId]  INT           NULL,
    [FID]         INT           NULL,
    [Amount]      MONEY         NULL,
    [IAccno]      INT           NULL,
    [Vno]         INT           NULL,
    [Remarks]     VARCHAR (200) NULL,
    CONSTRAINT [PK_DisburseCharge] PRIMARY KEY CLUSTERED ([DisChargeId] ASC),
    CONSTRAINT [FK_DisburseCharge_DisbursementMain] FOREIGN KEY ([DisburseId]) REFERENCES [fin].[DisbursementMain] ([DisburseId])
);

