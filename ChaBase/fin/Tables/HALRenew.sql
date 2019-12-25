CREATE TABLE [fin].[HALRenew] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [IAccNo]   INT           NULL,
    [Duration] INT           NULL,
    [StDate]   SMALLDATETIME NULL,
    [MDate]    SMALLDATETIME NULL,
    [PSID]     INT           NULL,
    [PF]       INT           NULL,
    [IF]       INT           NULL,
    [GMonthP]  INT           NULL,
    [GMonthI]  INT           NULL,
    [GDayP]    INT           NULL,
    [GDayI]    INT           NULL,
    [GDateP]   SMALLDATETIME NULL,
    [GDateI]   SMALLDATETIME NULL,
    [Days]     INT           NULL,
    [PMI]      INT           NULL,
    [Rate]     REAL          NULL,
    [RNDate]   SMALLDATETIME NULL,
    [Flag]     INT           NULL,
    CONSTRAINT [PK_HALRenew] PRIMARY KEY CLUSTERED ([ID] ASC)
);

