CREATE TABLE [acc].[Voucher1] (
    [V1Id]       INT           IDENTITY (1, 1) NOT NULL,
    [VNo]        INT           NOT NULL,
    [TDate]      DATETIME      NOT NULL,
    [ApprovedBy] INT           NULL,
    [ApprovedOn] DATETIME      NULL,
    [PostedBy]   INT           NOT NULL,
    [PostedOn]   DATETIME      CONSTRAINT [DF_Voucher1_PostedOn] DEFAULT (getdate()) NULL,
    [CTId]       INT           NOT NULL,
    [VNId]       INT           NOT NULL,
    [Narration]  VARCHAR (500) NULL,
    CONSTRAINT [PK_TempVoucher1] PRIMARY KEY CLUSTERED ([V1Id] ASC),
    CONSTRAINT [FK_Voucher1_CurrencyType] FOREIGN KEY ([CTId]) REFERENCES [acc].[CurrencyType] ([CTId])
);

