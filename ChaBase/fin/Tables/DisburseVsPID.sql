CREATE TABLE [fin].[DisburseVsPID] (
    [DisVsPID]   INT           IDENTITY (1, 1) NOT NULL,
    [DisburseId] INT           NOT NULL,
    [FID]        INT           NOT NULL,
    [Amount]     MONEY         NOT NULL,
    [Remarks]    VARCHAR (200) NULL,
    CONSTRAINT [PK_DisburseVsPID] PRIMARY KEY CLUSTERED ([DisVsPID] ASC),
    CONSTRAINT [FK_DisburseVsPID_DisbursementMain] FOREIGN KEY ([DisburseId]) REFERENCES [fin].[DisbursementMain] ([DisburseId])
);

