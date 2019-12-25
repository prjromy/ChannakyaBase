CREATE TABLE [fin].[ShrPurchase] (
    [Regno]      NUMERIC (18)   NOT NULL,
    [SCrtno]     NUMERIC (18)   NOT NULL,
    [Tno]        NUMERIC (18)   NOT NULL,
    [Pdate]      SMALLDATETIME  NOT NULL,
    [SQty]       NUMERIC (18)   NOT NULL,
    [Amt]        MONEY          NOT NULL,
    [Note]       NVARCHAR (150) NULL,
    [PostedBy]   INT            NOT NULL,
    [ApprovedBy] INT            NOT NULL,
    [ttype]      INT            NOT NULL,
    [brchid]     TINYINT        CONSTRAINT [DF_ShrPurchase_brchid] DEFAULT ((1)) NOT NULL,
    [Vno]        INT            NULL,
    CONSTRAINT [FK_ShrPurchase_SCrtDtls] FOREIGN KEY ([SCrtno]) REFERENCES [fin].[SCrtDtls] ([Sid]),
    CONSTRAINT [FK_ShrPurchase_ShrReg] FOREIGN KEY ([Regno]) REFERENCES [fin].[ShrReg] ([RegNo])
);

