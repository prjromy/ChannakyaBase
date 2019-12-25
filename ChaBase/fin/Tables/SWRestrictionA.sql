CREATE TABLE [fin].[SWRestrictionA] (
    [IAccNo] INT   NOT NULL,
    [Dur]    INT   NULL,
    [Amount] MONEY NULL,
    [FY]     INT   NOT NULL,
    CONSTRAINT [PK_SWRestrictionA] PRIMARY KEY CLUSTERED ([IAccNo] ASC, [FY] ASC)
);

