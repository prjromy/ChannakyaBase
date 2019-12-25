CREATE TABLE [fin].[RuleBalance] (
    [BALID]   TINYINT      IDENTITY (1, 1) NOT NULL,
    [BalRule] VARCHAR (17) NOT NULL,
    [Enable]  BIT          NULL,
    CONSTRAINT [PK_RuleBalance] PRIMARY KEY CLUSTERED ([BALID] ASC)
);

