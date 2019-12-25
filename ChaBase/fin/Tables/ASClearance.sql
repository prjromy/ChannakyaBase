CREATE TABLE [fin].[ASClearance] (
    [rno]           INT            IDENTITY (1, 1) NOT NULL,
    [IAccno]        INT            NOT NULL,
    [Bankname]      NVARCHAR (300) NOT NULL,
    [Brnhname]      NVARCHAR (300) NOT NULL,
    [chqno]         NVARCHAR (15)  NOT NULL,
    [payee]         NVARCHAR (50)  NULL,
    [tdate]         SMALLDATETIME  NOT NULL,
    [camount]       MONEY          NOT NULL,
    [remarks]       NVARCHAR (500) NULL,
    [postedby]      INT            NOT NULL,
    [accno]         NVARCHAR (50)  NULL,
    [VDate]         SMALLDATETIME  NOT NULL,
    [Postedon]      DATETIME       CONSTRAINT [DF_ASClearance_Postedon] DEFAULT (getdate()) NOT NULL,
    [chqstate]      BIT            NULL,
    [BrchId]        INT            NULL,
    [RejectRemarks] NVARCHAR (500) NULL,
    CONSTRAINT [PK_ASClearance] PRIMARY KEY CLUSTERED ([rno] ASC)
);

