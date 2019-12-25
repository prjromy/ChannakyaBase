CREATE TABLE [fin].[ForeignBuySell] (
    [TransNo]  NUMERIC (18)  NOT NULL,
    [Tdate]    SMALLDATETIME NOT NULL,
    [PostedBy] SMALLINT      NOT NULL,
    [InCurr]   SMALLINT      NULL,
    [DrAmt]    MONEY         NULL,
    [OutCurr]  SMALLINT      NULL,
    [CrAmt]    MONEY         NULL,
    [TNo]      NUMERIC (18)  NOT NULL,
    [IAccNo]   INT           NULL,
    [BrnhId]   INT           NOT NULL,
    [IVno]     NUMERIC (18)  NULL,
    [OVno]     NUMERIC (18)  NULL,
    [slpno]    INT           NULL,
    [notes]    NVARCHAR (50) NULL,
    [rate]     FLOAT (53)    NULL
);

