CREATE TABLE [fin].[ABal] (
    [IAccno] BIGINT          NOT NULL,
    [FId]    INT             NOT NULL,
    [Flag]   TINYINT         NULL,
    [Bal]    DECIMAL (18, 2) NOT NULL,
    [Id]     BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ABal] PRIMARY KEY CLUSTERED ([Id] ASC)
);

