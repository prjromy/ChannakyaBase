CREATE TABLE [fin].[AChq] (
    [rno]        INT           IDENTITY (1, 1) NOT NULL,
    [IAccno]     INT           NOT NULL,
    [cfrom]      INT           NOT NULL,
    [cto]        INT           NOT NULL,
    [cstate]     TINYINT       NOT NULL,
    [postedby]   INT           NOT NULL,
    [approvedby] INT           NULL,
    [tdate]      SMALLDATETIME NOT NULL,
    [IsPrinted]  BIT           NULL,
    CONSTRAINT [PK_AChq] PRIMARY KEY CLUSTERED ([rno] ASC),
    CONSTRAINT [FK_AChq_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

