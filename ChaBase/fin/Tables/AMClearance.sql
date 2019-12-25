CREATE TABLE [fin].[AMClearance] (
    [rno]        INT            NOT NULL,
    [IAccno]     INT            NOT NULL,
    [bankname]   NVARCHAR (300) NOT NULL,
    [brnhname]   NVARCHAR (300) NOT NULL,
    [chqno]      NVARCHAR (15)  NOT NULL,
    [payee]      NVARCHAR (50)  NULL,
    [tdate]      SMALLDATETIME  NOT NULL,
    [camount]    MONEY          NOT NULL,
    [remarks]    NVARCHAR (500) NULL,
    [postedby]   INT            NOT NULL,
    [accno]      NVARCHAR (50)  NULL,
    [VDate]      SMALLDATETIME  NOT NULL,
    [postedon]   DATETIME       NOT NULL,
    [verifiedby] INT            NOT NULL,
    [verifiedon] DATETIME       CONSTRAINT [DF_AMClearance_verifiedon] DEFAULT (getdate()) NOT NULL,
    [cdate]      SMALLDATETIME  NOT NULL,
    [BrchId]     INT            NULL,
    [VNo]        INT            NULL,
    CONSTRAINT [PK_AMClearance] PRIMARY KEY CLUSTERED ([rno] ASC)
);

