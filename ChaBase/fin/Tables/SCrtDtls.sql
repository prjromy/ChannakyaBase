CREATE TABLE [fin].[SCrtDtls] (
    [Sid]    NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [SCrtno] NUMERIC (18) NOT NULL,
    [FSno]   NUMERIC (18) NOT NULL,
    [TSno]   NUMERIC (18) NOT NULL,
    [SQty]   NUMERIC (18) NOT NULL,
    [Status] INT          NOT NULL,
    [Stype]  TINYINT      NOT NULL,
    CONSTRAINT [PK_SCrtDtls] PRIMARY KEY CLUSTERED ([Sid] ASC)
);

