CREATE TABLE [Trans].[ChgSTrnx] (
    [Tdate]      DATETIME       NOT NULL,
    [ChgId]      INT            NOT NULL,
    [Amt]        MONEY          NOT NULL,
    [TrnxNo]     BIGINT         NOT NULL,
    [Remarks]    NVARCHAR (200) NULL,
    [Postedby]   SMALLINT       NOT NULL,
    [Approvedby] SMALLINT       NULL,
    [Iaccno]     INT            NULL,
    [Vno]        SMALLINT       NULL,
    [ttype]      TINYINT        NULL,
    [slpno]      NUMERIC (18)   NULL,
    [brhid]      NUMERIC (18)   NULL,
    [IsDeleted]  BIT            NULL,
    CONSTRAINT [PK_ChgSTrnx] PRIMARY KEY CLUSTERED ([TrnxNo] ASC)
);

