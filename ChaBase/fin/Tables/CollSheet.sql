CREATE TABLE [fin].[CollSheet] (
    [CollSheetId] BIGINT         NOT NULL,
    [SheetNo]     INT            NOT NULL,
    [CollectorId] INT            NOT NULL,
    [TDate]       DATETIME       NOT NULL,
    [VDate]       DATETIME       NOT NULL,
    [PostedBy]    INT            NOT NULL,
    [ApprovedBy]  INT            NULL,
    [BrchId]      INT            NOT NULL,
    [TotalAmount] MONEY          NOT NULL,
    [note]        NVARCHAR (300) NULL,
    CONSTRAINT [PK_CollSheet] PRIMARY KEY CLUSTERED ([CollSheetId] ASC)
);

