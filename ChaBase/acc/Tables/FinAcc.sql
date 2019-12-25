CREATE TABLE [acc].[FinAcc] (
    [Fid]               INT             IDENTITY (1, 1) NOT NULL,
    [Fname]             VARCHAR (250)   NOT NULL,
    [Pid]               INT             NULL,
    [F2Type]            INT             NOT NULL,
    [Content]           VARBINARY (MAX) NULL,
    [DebitRestriction]  BIT             NULL,
    [CreditRestriction] BIT             NULL,
    [Code]              VARCHAR (10)    NULL,
    [Alias]             NVARCHAR (50)   NOT NULL,
    CONSTRAINT [PK__FinAcc__C1D1314AE2200241] PRIMARY KEY CLUSTERED ([Fid] ASC),
    CONSTRAINT [FK_FinAcc_FinSys2] FOREIGN KEY ([F2Type]) REFERENCES [acc].[FinSys2] ([F2id])
);

