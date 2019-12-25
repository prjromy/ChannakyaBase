CREATE TABLE [fin].[ProductDurationInt] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [PId]          INT             NOT NULL,
    [DBId]         INT             NULL,
    [ICBId]        INT             NULL,
    [DVId]         INT             NOT NULL,
    [Value]        FLOAT (53)      NULL,
    [InterestRate] DECIMAL (18, 3) NULL,
    CONSTRAINT [PK_InterestBasis] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductDurationInt_ProductDetail] FOREIGN KEY ([PId]) REFERENCES [fin].[ProductDetail] ([PID])
);

