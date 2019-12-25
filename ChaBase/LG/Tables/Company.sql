CREATE TABLE [LG].[Company] (
    [CompanyId]      INT             IDENTITY (1, 1) NOT NULL,
    [BranchName]     NVARCHAR (300)  NOT NULL,
    [Region]         NVARCHAR (4000) NULL,
    [State]          NVARCHAR (4000) NULL,
    [Address]        NVARCHAR (300)  NOT NULL,
    [PhoneNo]        NVARCHAR (4000) NULL,
    [Email]          NVARCHAR (300)  NOT NULL,
    [FaxNo]          NVARCHAR (4000) NULL,
    [Prefix]         NVARCHAR (300)  NOT NULL,
    [ParentId]       INT             NOT NULL,
    [IsBranch]       BIT             NULL,
    [AdditionalUser] INT             NOT NULL,
    [IPAddress]      NVARCHAR (300)  NOT NULL,
    [TDate]          DATETIME        NULL,
    CONSTRAINT [PK_LG.Company] PRIMARY KEY CLUSTERED ([CompanyId] ASC)
);

