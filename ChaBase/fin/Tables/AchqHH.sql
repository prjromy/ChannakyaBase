CREATE TABLE [fin].[AchqHH] (
    [AchqHHId] INT            IDENTITY (1, 1) NOT NULL,
    [AchqHId]  INT            NOT NULL,
    [Cstate]   TINYINT        NOT NULL,
    [Tdate]    SMALLDATETIME  NOT NULL,
    [Postedby] INT            NOT NULL,
    [PostedOn] DATETIME       NOT NULL,
    [Remarks]  NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_AchqHH] PRIMARY KEY CLUSTERED ([AchqHHId] ASC),
    CONSTRAINT [FK_AchqHH_AchqH] FOREIGN KEY ([AchqHId]) REFERENCES [fin].[AchqH] ([AchqHId])
);

