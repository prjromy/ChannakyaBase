CREATE TABLE [fin].[ARateMaster] (
    [ARMID]         INT      IDENTITY (1, 1) NOT NULL,
    [PostedBy]      INT      NOT NULL,
    [PostedDate]    DATETIME NOT NULL,
    [EffectiveDate] DATETIME NULL,
    CONSTRAINT [PK_ARateDetais] PRIMARY KEY CLUSTERED ([ARMID] ASC)
);

