CREATE TABLE [fin].[ChqAttributes] (
    [Id]                        INT            IDENTITY (1, 1) NOT NULL,
    [BranchId]                  INT            NULL,
    [AccountNamePostion]        NVARCHAR (128) NULL,
    [AccountNumberPosition]     NVARCHAR (128) NULL,
    [AccountTypePosition]       NVARCHAR (128) NULL,
    [ChequeNumberPosition]      NVARCHAR (128) NULL,
    [Cheque2NumberPosition]     NVARCHAR (128) NULL,
    [BranchPhoneNumberPosition] NVARCHAR (50)  NULL,
    [BranchNamePosition]        NVARCHAR (128) NULL,
    [BranchAddressPosition]     NVARCHAR (50)  NULL,
    [DefaultPrinterName]        NVARCHAR (50)  NULL,
    [PaperSize]                 NVARCHAR (50)  NULL,
    CONSTRAINT [PK_ChqAttributes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

