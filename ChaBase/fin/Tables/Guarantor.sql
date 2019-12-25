CREATE TABLE [fin].[Guarantor] (
    [GId]        INT      IDENTITY (1, 1) NOT NULL,
    [LIaccno]    INT      NOT NULL,
    [GIaccno]    INT      NOT NULL,
    [BlockedAmt] MONEY    NULL,
    [DisplayMsg] BIT      NOT NULL,
    [IsPercent]  BIT      NOT NULL,
    [Status]     BIT      NOT NULL,
    [PostedBy]   INT      NOT NULL,
    [PostedOn]   DATETIME NOT NULL,
    [Sno]        INT      NULL,
    CONSTRAINT [PK_Guarantor] PRIMARY KEY CLUSTERED ([GId] ASC),
    CONSTRAINT [FK_Guarantor_ADetail] FOREIGN KEY ([LIaccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

