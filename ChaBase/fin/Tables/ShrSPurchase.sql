CREATE TABLE [fin].[ShrSPurchase] (
    [Regno]     NUMERIC (18)   NOT NULL,
    [Tno]       NUMERIC (18)   NOT NULL,
    [Pdate]     SMALLDATETIME  NOT NULL,
    [SQty]      NUMERIC (18)   NOT NULL,
    [Amt]       MONEY          NOT NULL,
    [Note]      NVARCHAR (500) NULL,
    [PostedBy]  INT            NOT NULL,
    [ttype]     INT            NOT NULL,
    [SType]     INT            NULL,
    [Vno]       INT            NULL,
    [Brchid]    TINYINT        NULL,
    [IsDeleted] BIT            NOT NULL,
    CONSTRAINT [PK_ShrSPurchase_1] PRIMARY KEY CLUSTERED ([Tno] ASC),
    CONSTRAINT [FK_ShrSPurchase_ShrReg] FOREIGN KEY ([Regno]) REFERENCES [fin].[ShrReg] ([RegNo])
);

