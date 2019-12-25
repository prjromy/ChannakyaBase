CREATE TABLE [fin].[AintPayable] (
    [Tno]        BIGINT         NOT NULL,
    [Tdate]      DATETIME       NOT NULL,
    [Iaccno]     INT            NOT NULL,
    [IntAmt]     MONEY          NOT NULL,
    [TaxRt]      REAL           NOT NULL,
    [Tax]        AS             (CONVERT([money],round(([Intamt]*[taxrt])/(100),(2)))),
    [CRamt]      AS             (CONVERT([money],[intamt]-round(([Intamt]*[taxrt])/(100),(2)))),
    [DrAmt]      MONEY          NULL,
    [Ivno]       INT            NULL,
    [Tvno]       INT            NULL,
    [Valued]     SMALLINT       NOT NULL,
    [PostedBy]   INT            NULL,
    [VerifiedBy] INT            NULL,
    [ByTeller]   BIT            NOT NULL,
    [Remarks]    NVARCHAR (100) NULL,
    CONSTRAINT [PK_AintPayable] PRIMARY KEY CLUSTERED ([Tno] ASC),
    CONSTRAINT [FK_AintPayable_ADetail] FOREIGN KEY ([Iaccno]) REFERENCES [fin].[ADetail] ([IAccno]) ON DELETE CASCADE ON UPDATE CASCADE
);

