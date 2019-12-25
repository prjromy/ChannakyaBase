CREATE TABLE [fin].[ALFixedDeposit] (
    [AlFixedId]  INT           IDENTITY (1, 1) NOT NULL,
    [Sno]        INT           NOT NULL,
    [IsInternal] BIT           NOT NULL,
    [fdAccno]    NVARCHAR (50) NOT NULL,
    [amount]     MONEY         NULL,
    [openDt]     DATETIME      NULL,
    [matDt]      DATETIME      NULL,
    [bank]       VARCHAR (35)  NULL,
    [brnh]       VARCHAR (35)  NULL,
    CONSTRAINT [PK_ALFixedDeposit] PRIMARY KEY CLUSTERED ([AlFixedId] ASC),
    CONSTRAINT [FK_ALFixedDeposit_ALColl] FOREIGN KEY ([Sno]) REFERENCES [fin].[ALColl] ([Sno])
);

