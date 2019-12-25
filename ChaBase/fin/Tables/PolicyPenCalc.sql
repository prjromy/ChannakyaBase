CREATE TABLE [fin].[PolicyPenCalc] (
    [PCID]   TINYINT      IDENTITY (1, 1) NOT NULL,
    [PCNAME] VARCHAR (30) NOT NULL,
    [RID]    TINYINT      NOT NULL,
    [PBALID] TINYINT      NOT NULL,
    [DURID]  TINYINT      NOT NULL,
    CONSTRAINT [PK_PolicyPenCalc] PRIMARY KEY CLUSTERED ([PCID] ASC),
    CONSTRAINT [FK_PolicyPenCalc_RuleDuration] FOREIGN KEY ([DURID]) REFERENCES [fin].[RuleDuration] ([DURID]),
    CONSTRAINT [FK_PolicyPenCalc_RulePenBalance] FOREIGN KEY ([PBALID]) REFERENCES [fin].[RulePenBalance] ([PBALID]),
    CONSTRAINT [FK_PolicyPenCalc_RuleRate] FOREIGN KEY ([RID]) REFERENCES [fin].[RuleRate] ([RID])
);

