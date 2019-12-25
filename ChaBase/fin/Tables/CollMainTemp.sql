CREATE TABLE [fin].[CollMainTemp] (
    [RetId]           INT           IDENTITY (1, 1) NOT NULL,
    [BrchId]          INT           NOT NULL,
    [CollectionAmt]   MONEY         NOT NULL,
    [CollectorId]     INT           NOT NULL,
    [TDate]           SMALLDATETIME NOT NULL,
    [VDate]           SMALLDATETIME NOT NULL,
    [PostedBy]        INT           NOT NULL,
    [TotalAmt]        MONEY         NOT NULL,
    [SheetNo]         INT           NOT NULL,
    [RejectedBy]      INT           NULL,
    [Status]          BIT           NOT NULL,
    [RejectedRemarks] VARCHAR (200) NULL,
    CONSTRAINT [PK_CollMainTemp] PRIMARY KEY CLUSTERED ([RetId] ASC)
);

