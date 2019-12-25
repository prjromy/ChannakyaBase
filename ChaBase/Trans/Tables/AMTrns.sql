CREATE TABLE [Trans].[AMTrns] (
    [tno]        NUMERIC (18)    NOT NULL,
    [IAccno]     INT             NOT NULL,
    [tdate]      SMALLDATETIME   NOT NULL,
    [vdate]      SMALLDATETIME   NOT NULL,
    [notes]      NVARCHAR (1000) NULL,
    [slpno]      INT             NULL,
    [dramt]      NUMERIC (19, 2) NOT NULL,
    [cramt]      NUMERIC (19, 2) NOT NULL,
    [ttype]      INT             NOT NULL,
    [postedby]   INT             NOT NULL,
    [approvedby] INT             NOT NULL,
    [brnhno]     INT             NOT NULL,
    [vno]        INT             NULL,
    [Isslp]      BIT             NOT NULL,
    [PostedOn]   DATETIME        NULL,
    CONSTRAINT [PK_AMTrns] PRIMARY KEY CLUSTERED ([tno] ASC)
);

