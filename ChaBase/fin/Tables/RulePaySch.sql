CREATE TABLE [fin].[RulePaySch] (
    [PAYSID]  TINYINT      NOT NULL,
    [schType] VARCHAR (25) NOT NULL,
    [active]  BIT          NOT NULL,
    CONSTRAINT [PK_RulePaySch] PRIMARY KEY CLUSTERED ([PAYSID] ASC)
);

