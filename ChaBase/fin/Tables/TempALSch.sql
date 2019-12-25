CREATE TABLE [fin].[TempALSch] (
    [AlshId]  DECIMAL (18)  IDENTITY (1, 1) NOT NULL,
    [IAccno]  INT           NOT NULL,
    [schDate] SMALLDATETIME NULL,
    [schPrin] MONEY         NULL,
    [schInt]  MONEY         NULL,
    [calcInt] MONEY         NULL,
    [balPrin] MONEY         NULL,
    [pdPrin]  MONEY         NULL,
    [pdInt]   MONEY         NULL,
    CONSTRAINT [PK_TempALSch] PRIMARY KEY CLUSTERED ([AlshId] ASC),
    CONSTRAINT [CK_TempALSch] CHECK ([Pdint] IS NULL OR [pdint]>=(0)),
    CONSTRAINT [CK_TempALSch_1] CHECK ([pdprin] IS NULL OR [pdprin]>=(0)),
    CONSTRAINT [CK_TempALSch_2] CHECK ([calcint] IS NULL OR [calcint]>=(0)),
    CONSTRAINT [FK_TempALSch_AccountDetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

