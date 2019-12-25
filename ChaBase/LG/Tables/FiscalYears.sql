CREATE TABLE [LG].[FiscalYears] (
    [FYID]    SMALLINT       IDENTITY (1, 1) NOT NULL,
    [FyName]  NVARCHAR (MAX) NULL,
    [StartDt] DATETIME       NOT NULL,
    [EndDt]   DATETIME       NOT NULL,
    CONSTRAINT [PK_LG.FiscalYears] PRIMARY KEY CLUSTERED ([FYID] ASC)
);

