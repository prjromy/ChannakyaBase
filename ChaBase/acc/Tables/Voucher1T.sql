CREATE TABLE [acc].[Voucher1T] (
    [V1TId]      INT           IDENTITY (1, 1) NOT NULL,
    [VNo]        INT           NOT NULL,
    [TDate]      DATETIME      NOT NULL,
    [PostedBy]   INT           NOT NULL,
    [PostedOn]   DATETIME      NOT NULL,
    [CTId]       INT           NOT NULL,
    [VNId]       INT           NOT NULL,
    [Narration]  VARCHAR (100) NULL,
    [VerifiedOn] DATETIME      NOT NULL,
    CONSTRAINT [PK_Voucher1T] PRIMARY KEY CLUSTERED ([V1TId] ASC),
    CONSTRAINT [FK_Voucher1T_CurrencyType] FOREIGN KEY ([CTId]) REFERENCES [acc].[CurrencyType] ([CTId])
);

