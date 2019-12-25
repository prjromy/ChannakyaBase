CREATE TABLE [fin].[AchqH] (
    [AchqHId] INT     IDENTITY (1, 1) NOT NULL,
    [Rno]     INT     NOT NULL,
    [ChkNo]   INT     NOT NULL,
    [cstate]  TINYINT NOT NULL,
    [Tno]     BIGINT  NULL,
    CONSTRAINT [PK_AchqH] PRIMARY KEY CLUSTERED ([AchqHId] ASC),
    CONSTRAINT [FK_AchqH_AChq] FOREIGN KEY ([Rno]) REFERENCES [fin].[AChq] ([rno])
);

