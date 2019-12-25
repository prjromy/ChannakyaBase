CREATE TABLE [fin].[RuleRenew] (
    [RNWID]     TINYINT      IDENTITY (1, 1) NOT NULL,
    [RenewRule] VARCHAR (50) NOT NULL,
    [enable]    BIT          NOT NULL,
    CONSTRAINT [PK_RuleRenew] PRIMARY KEY CLUSTERED ([RNWID] ASC)
);

