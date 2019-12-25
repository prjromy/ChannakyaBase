CREATE TABLE [fin].[ShrReturn] (
    [Regno]      NUMERIC (18)   NOT NULL,
    [SCrtno]     NUMERIC (18)   NOT NULL,
    [SoldTo]     NUMERIC (18)   NULL,
    [Tno]        NUMERIC (18)   NOT NULL,
    [Sdate]      DATETIME       NOT NULL,
    [SQty]       NUMERIC (18)   NOT NULL,
    [Amt]        MONEY          NULL,
    [Note]       NVARCHAR (150) NULL,
    [PostedBy]   INT            NULL,
    [ApprovedBy] INT            NOT NULL,
    [ttype]      INT            NOT NULL,
    [brchid]     TINYINT        CONSTRAINT [DF_ShrReturn_brchid] DEFAULT ((1)) NOT NULL,
    [Vno]        INT            NULL
);

