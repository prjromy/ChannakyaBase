CREATE TABLE [Mast].[SheetNo] (
    [UTId]      INT          IDENTITY (1, 1) NOT NULL,
    [FYId]      INT          NOT NULL,
    [TransType] VARCHAR (50) NOT NULL,
    [BranchId]  INT          NOT NULL,
    [CurrentNo] INT          NOT NULL,
    CONSTRAINT [PK_UTransNo] PRIMARY KEY CLUSTERED ([UTId] ASC)
);

