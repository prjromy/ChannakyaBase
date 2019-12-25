CREATE TABLE [fin].[ALinkloan] (
    [IAccno]   INT     NOT NULL,
    [ILinkAc]  INT     NOT NULL,
    [priority] TINYINT NOT NULL,
    [Id]       INT     IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ALinkloan] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ALinkloan_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

