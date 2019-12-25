CREATE TABLE [fin].[DenoBal] (
    [DenoBalId] INT IDENTITY (1, 1) NOT NULL,
    [UserId]    INT NULL,
    [UserLevel] INT NULL,
    [DenoId]    INT NOT NULL,
    [Piece]     INT NOT NULL,
    CONSTRAINT [PK_DenoBal] PRIMARY KEY CLUSTERED ([DenoBalId] ASC),
    CONSTRAINT [FK_DenoBal_Denosetup] FOREIGN KEY ([DenoId]) REFERENCES [fin].[Denosetup] ([DenoID])
);

