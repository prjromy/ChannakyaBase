CREATE TABLE [fin].[ALoanGrace] (
    [IAccno]      INT           NOT NULL,
    [GDurP]       SMALLINT      NULL,
    [GDurI]       SMALLINT      NULL,
    [GDateP]      SMALLDATETIME NULL,
    [GDateI]      SMALLDATETIME NULL,
    [GDayP]       SMALLINT      NULL,
    [GDayI]       SMALLINT      NULL,
    [GraceOption] INT           NOT NULL,
    CONSTRAINT [PK_ALoanGrace] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_ALoanGrace_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

