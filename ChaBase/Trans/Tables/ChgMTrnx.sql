CREATE TABLE [Trans].[ChgMTrnx] (
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
    CONSTRAINT [PK_ChgMTrnx] PRIMARY KEY CLUSTERED ([TrnxNo] ASC)
);

