CREATE TABLE [fin].[ALSchHistry] (
    [ALSchHId]  INT             IDENTITY (1, 1) NOT NULL,
    [IAccno]    INT             NOT NULL,
    [schDate]   SMALLDATETIME   NULL,
    [schPrin]   NUMERIC (19, 2) NULL,
    [schInt]    NUMERIC (19, 2) NULL,
    [calcInt]   NUMERIC (19, 2) NULL,
    [balPrin]   NUMERIC (19, 2) NULL,
    [pdPrin]    NUMERIC (19, 2) NULL,
    [pdInt]     NUMERIC (19, 2) NULL,
    [reSchFreq] INT             NULL,
    CONSTRAINT [PK_ALSchHistry] PRIMARY KEY CLUSTERED ([ALSchHId] ASC)
);

