CREATE TABLE [fin].[RulePenBalance] (
    [PBALID]   TINYINT      IDENTITY (1, 1) NOT NULL,
    [PBalance] VARCHAR (15) NULL,
    [Enable]   BIT          NULL,
    CONSTRAINT [PK_RulePenBalance] PRIMARY KEY CLUSTERED ([PBALID] ASC)
);

