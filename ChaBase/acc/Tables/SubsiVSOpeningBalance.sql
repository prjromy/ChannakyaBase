CREATE TABLE [acc].[SubsiVSOpeningBalance] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [FId]        INT             NOT NULL,
    [SubsiId]    INT             NOT NULL,
    [FYID]       INT             NULL,
    [OpeningBal] DECIMAL (18, 2) NOT NULL,
    [ClosingBal] DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_SubsiVSOpeningBalance] PRIMARY KEY CLUSTERED ([Id] ASC)
);

