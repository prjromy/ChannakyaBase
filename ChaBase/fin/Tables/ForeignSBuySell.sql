CREATE TABLE [fin].[ForeignSBuySell] (
    [TransNo]  NUMERIC (18)  NOT NULL,
    [Tdate]    SMALLDATETIME NOT NULL,
    [PostedBy] SMALLINT      NOT NULL,
    [InCurr]   SMALLINT      NOT NULL,
    [DrAmt]    MONEY         NOT NULL,
    [OutCurr]  SMALLINT      NOT NULL,
    [CrAmt]    MONEY         NOT NULL,
    [TNo]      NUMERIC (18)  NOT NULL,
    [IAccNo]   INT           NOT NULL,
    [BrnhId]   INT           NOT NULL,
    [IVno]     NUMERIC (18)  NULL,
    [OVno]     NUMERIC (18)  NULL,
    [slpno]    INT           NULL,
    [IsSlp]    BIT           NULL,
    [notes]    NVARCHAR (50) NULL,
    [rate]     FLOAT (53)    NULL,
    CONSTRAINT [PK_ForeignSBuySell] PRIMARY KEY CLUSTERED ([TransNo] ASC)
);

