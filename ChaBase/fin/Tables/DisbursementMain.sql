CREATE TABLE [fin].[DisbursementMain] (
    [DisburseId]    INT      IDENTITY (1, 1) NOT NULL,
    [IAccno]        INT      NOT NULL,
    [Mode]          INT      NOT NULL,
    [VNo]           INT      NULL,
    [Tdate]         DATE     NULL,
    [PostedOn]      DATETIME CONSTRAINT [DF_DisbursementMain_PostedOn] DEFAULT (getdate()) NOT NULL,
    [PostedBy]      INT      NULL,
    [ApprovedOn]    DATETIME NULL,
    [ApprovedBy]    INT      NULL,
    [CheckSanction] BIT      NULL,
    CONSTRAINT [PK_DisbursementMain] PRIMARY KEY CLUSTERED ([DisburseId] ASC),
    CONSTRAINT [FK_DisbursementMain_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

