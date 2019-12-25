CREATE TABLE [acc].[InterestRule] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [RuleName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_InterestRuleID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

