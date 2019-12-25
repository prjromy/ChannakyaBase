CREATE TABLE [fin].[ALSch] (
    [AlshId]  DECIMAL (18)  IDENTITY (1, 1) NOT NULL,
    [IAccno]  INT           NOT NULL,
    [schDate] SMALLDATETIME NULL,
    [schPrin] MONEY         NULL,
    [schInt]  MONEY         NULL,
    [calcInt] MONEY         NULL,
    [balPrin] MONEY         NULL,
    [pdPrin]  MONEY         NULL,
    [pdInt]   MONEY         NULL,
    CONSTRAINT [PK_ALSch] PRIMARY KEY CLUSTERED ([AlshId] ASC),
    CONSTRAINT [CK_ALSch] CHECK ([Pdint] IS NULL OR [pdint]>=(0)),
    CONSTRAINT [CK_ALSch_1] CHECK ([pdprin] IS NULL OR [pdprin]>=(0)),
    CONSTRAINT [CK_ALSch_2] CHECK ([calcint] IS NULL OR [calcint]>=(0)),
    CONSTRAINT [FK_AccountSch_AccountDetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

