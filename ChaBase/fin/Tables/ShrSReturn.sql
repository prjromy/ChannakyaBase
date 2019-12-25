CREATE TABLE [fin].[ShrSReturn] (
    [Regno]     NUMERIC (18)   NOT NULL,
    [SCrtNo]    NUMERIC (18)   NOT NULL,
    [SoldTo]    NUMERIC (18)   NULL,
    [Tno]       NUMERIC (18)   NOT NULL,
    [Sdate]     DATETIME       NOT NULL,
    [SQty]      NUMERIC (18)   NOT NULL,
    [Amt]       MONEY          NOT NULL,
    [Note]      NVARCHAR (150) NULL,
    [PostedBy]  INT            NOT NULL,
    [ttype]     INT            NOT NULL,
    [SType]     INT            NOT NULL,
    [Vno]       INT            NULL,
    [Brchid]    TINYINT        NULL,
    [Isdeleted] BIT            NOT NULL,
    CONSTRAINT [PK__ShrSRetu__C450026D6638E816] PRIMARY KEY CLUSTERED ([Tno] ASC)
);

