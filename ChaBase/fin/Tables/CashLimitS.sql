CREATE TABLE [fin].[CashLimitS] (
    [cashLimitId] INT   IDENTITY (1, 1) NOT NULL,
    [stfid]       INT   NOT NULL,
    [dramt]       MONEY NOT NULL,
    [cramt]       MONEY NOT NULL,
    [BranchId]    INT   NOT NULL,
    CONSTRAINT [PK_CashLimitS] PRIMARY KEY CLUSTERED ([cashLimitId] ASC),
    CONSTRAINT [FK_CashLimitS_CashLimitS] FOREIGN KEY ([cashLimitId]) REFERENCES [fin].[CashLimitS] ([cashLimitId]),
    CONSTRAINT [FK_CashLimitS_Company] FOREIGN KEY ([BranchId]) REFERENCES [LG].[Company] ([CompanyId])
);

