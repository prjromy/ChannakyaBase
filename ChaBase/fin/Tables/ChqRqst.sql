CREATE TABLE [fin].[ChqRqst] (
    [Rno]      INT      IDENTITY (1, 1) NOT NULL,
    [Iaccno]   INT      NOT NULL,
    [Pics]     INT      NOT NULL,
    [Tdate]    DATETIME NOT NULL,
    [PostedBy] INT      NOT NULL,
    CONSTRAINT [PK_ChqRqst] PRIMARY KEY CLUSTERED ([Rno] ASC),
    CONSTRAINT [FK_ChqRqst_ADetail] FOREIGN KEY ([Iaccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

