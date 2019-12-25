CREATE TABLE [fin].[ALRegCusts] (
    [ALRegID] INT IDENTITY (1, 1) NOT NULL,
    [RegId]   INT NOT NULL,
    [CID]     INT NULL,
    [SNo]     INT NULL,
    CONSTRAINT [PK_ALRegCusts] PRIMARY KEY CLUSTERED ([ALRegID] ASC),
    CONSTRAINT [FK_ALRegCusts_ALRegistration1] FOREIGN KEY ([RegId]) REFERENCES [fin].[ALRegistration] ([RegId])
);

