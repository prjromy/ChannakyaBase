CREATE TABLE [fin].[Denosetup] (
    [DenoID] INT        IDENTITY (1, 1) NOT NULL,
    [CurrID] SMALLINT   NOT NULL,
    [Deno]   FLOAT (53) NOT NULL,
    CONSTRAINT [PK_Denosetup] PRIMARY KEY CLUSTERED ([DenoID] ASC)
);

