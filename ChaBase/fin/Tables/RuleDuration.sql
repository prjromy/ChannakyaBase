CREATE TABLE [fin].[RuleDuration] (
    [DURID]   TINYINT      IDENTITY (1, 1) NOT NULL,
    [DurRule] VARCHAR (15) NOT NULL,
    [Enable]  BIT          NULL,
    CONSTRAINT [PK_RuleDuration] PRIMARY KEY CLUSTERED ([DURID] ASC)
);

