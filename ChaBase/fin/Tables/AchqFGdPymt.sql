CREATE TABLE [fin].[AchqFGdPymt] (
    [tno]        NUMERIC (18)   NOT NULL,
    [IAccno]     INT            NOT NULL,
    [brnhid]     INT            NOT NULL,
    [Chqno]      INT            NOT NULL,
    [notes]      NVARCHAR (200) NULL,
    [amount]     MONEY          NOT NULL,
    [tdate]      SMALLDATETIME  NOT NULL,
    [postedby]   INT            NOT NULL,
    [approvedby] INT            NULL,
    [tstate]     TINYINT        NOT NULL,
    [payee]      NVARCHAR (50)  NULL,
    [IsRejected] BIT            NULL,
    CONSTRAINT [PK_AchqFGdPymt] PRIMARY KEY CLUSTERED ([tno] ASC),
    CONSTRAINT [FK_AchqFGdPymt_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno]),
    CONSTRAINT [FK_AchqFGdPymt_Company] FOREIGN KEY ([brnhid]) REFERENCES [LG].[Company] ([CompanyId])
);

