CREATE TABLE [fin].[AintCap] (
    [Vdate]     SMALLDATETIME   NOT NULL,
    [TIaccno]   INT             NOT NULL,
    [Trxno]     INT             NOT NULL,
    [IntAmt]    NUMERIC (19, 2) NOT NULL,
    [TaxRt]     REAL            NOT NULL,
    [Tax]       AS              (CONVERT([money],round(([Taxrt]/(100))*[fin].[Fisneg]([IntAmt],(0)),(2)))),
    [CRamt]     AS              (CONVERT([money],[intamt]-round(([Taxrt]/(100))*[fin].[Fisneg]([IntAmt],(0)),(2)))),
    [FIaccno]   INT             NULL,
    [Ivno]      INT             NULL,
    [Tvno]      INT             NULL,
    [IsSlf]     BIT             NULL,
    [AintCapId] DECIMAL (18)    IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_AintCap] PRIMARY KEY CLUSTERED ([AintCapId] ASC)
);

