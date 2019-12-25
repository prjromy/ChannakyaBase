CREATE TABLE [fin].[RuleRate] (
    [RID]      TINYINT      IDENTITY (1, 1) NOT NULL,
    [RateRule] VARCHAR (30) NOT NULL,
    [Enable]   BIT          NULL,
    CONSTRAINT [PK_RuleRate] PRIMARY KEY CLUSTERED ([RID] ASC)
);

