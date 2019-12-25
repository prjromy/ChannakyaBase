CREATE TABLE [fin].[IChkDep] (
    [Tno]        BIGINT        NOT NULL,
    [FIaccno]    INT           NOT NULL,
    [TIAccno]    INT           NOT NULL,
    [Tdate]      SMALLDATETIME NOT NULL,
    [Vdate]      SMALLDATETIME NULL,
    [Amt]        MONEY         NOT NULL,
    [Ttype]      INT           NOT NULL,
    [SlpNo]      INT           NOT NULL,
    [postedby]   INT           NOT NULL,
    [verifiedby] INT           NULL,
    [Vno]        INT           NULL,
    [AVno]       NUMERIC (18)  NULL,
    [IBChq]      BIT           NULL,
    [BrchID]     INT           NOT NULL,
    CONSTRAINT [PK_IChkDep] PRIMARY KEY CLUSTERED ([Tno] ASC),
    CONSTRAINT [FK_IChkDep_ADetail] FOREIGN KEY ([FIaccno]) REFERENCES [fin].[ADetail] ([IAccno]) ON UPDATE CASCADE,
    CONSTRAINT [FK_IChkDep_ADetail1] FOREIGN KEY ([TIAccno]) REFERENCES [fin].[ADetail] ([IAccno]),
    CONSTRAINT [FK_IChkDep_Company] FOREIGN KEY ([BrchID]) REFERENCES [LG].[Company] ([CompanyId])
);

