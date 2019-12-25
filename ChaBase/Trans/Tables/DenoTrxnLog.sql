CREATE TABLE [Trans].[DenoTrxnLog] (
    [Id]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [Trxno]  NUMERIC (18)  NOT NULL,
    [vdate]  SMALLDATETIME NOT NULL,
    [Denoid] INT           NOT NULL,
    [Pics]   INT           NOT NULL,
    [UserId] INT           NOT NULL,
    CONSTRAINT [PK_DenoTrxn_2] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DenoTrxnCopy_Denosetup] FOREIGN KEY ([Denoid]) REFERENCES [fin].[Denosetup] ([DenoID])
);

