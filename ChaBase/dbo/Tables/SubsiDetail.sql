CREATE TABLE [dbo].[SubsiDetail] (
    [SDID]        INT             IDENTITY (1, 1) NOT NULL,
    [CId]         INT             NOT NULL,
    [FId]         INT             NOT NULL,
    [AccNo]       VARCHAR (50)    NULL,
    [Status]      BIT             NULL,
    [Enable]      BIT             NULL,
    [DebitLimit]  NUMERIC (18, 2) NULL,
    [CreditLimit] NUMERIC (18, 2) NULL,
    [PostedBy]    INT             NOT NULL,
    [PostedOn]    DATETIME        NOT NULL,
    [BranchId]    INT             NOT NULL,
    CONSTRAINT [PK_SubsiDetail] PRIMARY KEY CLUSTERED ([SDID] ASC)
);

