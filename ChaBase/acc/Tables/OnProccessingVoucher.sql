﻿CREATE TABLE [acc].[OnProccessingVoucher] (
    [ID]           INT          IDENTITY (1, 1) NOT NULL,
    [UserID]       INT          NULL,
    [TempId]       INT          NULL,
    [Fid]          INT          NULL,
    [Particulars]  VARCHAR (50) NULL,
    [Description]  VARCHAR (50) NULL,
    [DebitAmount]  MONEY        NULL,
    [CreditAmount] MONEY        NULL,
    [BranchID]     INT          NULL,
    [DivisionID]   INT          NULL,
    [SectionID]    INT          NULL,
    [DepartmentID] NCHAR (10)   NULL,
    [CostCenterID] INT          NULL,
    [SLId]         INT          NULL,
    [ChequeNo]     VARCHAR (50) NULL,
    [ChequeDate]   DATE         NULL,
    [ChequeAmount] MONEY        NULL,
    [Payee]        VARCHAR (50) NULL,
    [TV2Id]        INT          NULL,
    [DueDate]      DATE         NULL,
    [EntryDate]    DATETIME     NULL,
    CONSTRAINT [PK__OnProcce__3214EC27CAF1E1E3] PRIMARY KEY CLUSTERED ([ID] ASC)
);
