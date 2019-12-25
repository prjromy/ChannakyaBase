CREATE TABLE [dbo].[InterestMinimumBalance] (
    [ID]             INT   IDENTITY (1, 1) NOT NULL,
    [MinimumBalance] MONEY NULL,
    CONSTRAINT [PK_InterestMinimumBalance] PRIMARY KEY CLUSTERED ([ID] ASC)
);

