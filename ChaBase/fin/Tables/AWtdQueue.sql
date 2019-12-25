CREATE TABLE [fin].[AWtdQueue] (
    [Rno]        INT            IDENTITY (1, 1) NOT NULL,
    [IAccno]     INT            NOT NULL,
    [tdate]      SMALLDATETIME  NOT NULL,
    [chqno]      NUMERIC (18)   NOT NULL,
    [notes]      NVARCHAR (150) NULL,
    [amount]     MONEY          NOT NULL,
    [ttype]      INT            NULL,
    [postedby]   INT            NOT NULL,
    [Approvedby] INT            NULL,
    [BrnhID]     INT            NOT NULL,
    [currid]     INT            NULL,
    [isslip]     BIT            NOT NULL,
    [tno]        NUMERIC (18)   NULL,
    [Qstate]     TINYINT        NOT NULL,
    CONSTRAINT [PK_AWtdQueue] PRIMARY KEY CLUSTERED ([Rno] ASC),
    CONSTRAINT [FK_AWtdQueue_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

