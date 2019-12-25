CREATE TABLE [acc].[BankInfo] (
    [Bid]            INT          IDENTITY (1, 1) NOT NULL,
    [Fid]            INT          NOT NULL,
    [PhoneNo]        VARCHAR (15) NOT NULL,
    [Branch]         VARCHAR (20) NOT NULL,
    [ContactPerson]  VARCHAR (50) NOT NULL,
    [IsFixed]        BIT          NOT NULL,
    [InterestRate]   FLOAT (53)   NULL,
    [OpenDate]       DATETIME     NOT NULL,
    [MatureDate]     DATETIME     NULL,
    [InterestRuleId] INT          NULL,
    [MinimumBalance] MONEY        NULL,
    [AccountNo]      VARCHAR (50) NULL,
    [ForLiquidity]   BIT          DEFAULT ((0)) NULL,
    [BranchId]       INT          NOT NULL,
    [IsActive]       BIT          DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__BankInfo__C6D111C9D3C213AA] PRIMARY KEY CLUSTERED ([Bid] ASC),
    CONSTRAINT [FK__BankInfo__Fid__173876EA] FOREIGN KEY ([Fid]) REFERENCES [acc].[FinAcc] ([Fid]),
    CONSTRAINT [FK_BankInfo_FinAcc] FOREIGN KEY ([Fid]) REFERENCES [acc].[FinAcc] ([Fid]),
    CONSTRAINT [FK_BankInfo_InterestRuleID] FOREIGN KEY ([InterestRuleId]) REFERENCES [acc].[InterestRule] ([ID])
);

