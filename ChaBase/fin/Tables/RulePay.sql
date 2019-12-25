CREATE TABLE [fin].[RulePay] (
    [PAYID]  TINYINT      IDENTITY (1, 1) NOT NULL,
    [PRule]  VARCHAR (35) NOT NULL,
    [active] BIT          NOT NULL,
    CONSTRAINT [PK_PayRules] PRIMARY KEY CLUSTERED ([PAYID] ASC)
);

