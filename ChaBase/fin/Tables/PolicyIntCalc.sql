CREATE TABLE [fin].[PolicyIntCalc] (
    [PSID]   TINYINT       IDENTITY (1, 1) NOT NULL,
    [PSName] NVARCHAR (30) NOT NULL,
    [RID]    TINYINT       NOT NULL,
    [BALID]  TINYINT       NOT NULL,
    [DURID]  TINYINT       NOT NULL,
    CONSTRAINT [PK_RateScheme] PRIMARY KEY CLUSTERED ([PSID] ASC),
    CONSTRAINT [FK_PolicyIntCalc_RuleBalance] FOREIGN KEY ([BALID]) REFERENCES [fin].[RuleBalance] ([BALID]),
    CONSTRAINT [FK_PolicyIntCalc_RuleDuration] FOREIGN KEY ([DURID]) REFERENCES [fin].[RuleDuration] ([DURID]),
    CONSTRAINT [FK_PolicyIntCalc_RuleRate] FOREIGN KEY ([RID]) REFERENCES [fin].[RuleRate] ([RID])
);

