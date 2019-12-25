CREATE TABLE [fin].[InterestLogs] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [PId]           INT             NOT NULL,
    [DBId]          INT             NULL,
    [ICBId]         INT             NULL,
    [DVId]          INT             NULL,
    [Value]         FLOAT (53)      NULL,
    [EffectiveFrom] DATETIME        NOT NULL,
    [InterestRate]  DECIMAL (18, 3) NOT NULL,
    [OIRate]        REAL            NULL,
    [PPRate]        REAL            NULL,
    [PIRate]        REAL            NULL,
    [PostedBy]      INT             NOT NULL,
    [PostedOn]      DATETIME        NOT NULL,
    [isApplied]     BIT             NULL,
    CONSTRAINT [PK_InterestLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

