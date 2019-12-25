CREATE TABLE [Mast].[Task] (
    [Task1Id]    INT           IDENTITY (1, 1) NOT NULL,
    [RaisedOn]   DATETIME      CONSTRAINT [DF_Task_RaisedOn] DEFAULT (getdate()) NULL,
    [RaisedBy]   INT           NOT NULL,
    [EventId]    INT           NOT NULL,
    [EventValue] BIGINT        NOT NULL,
    [Message]    VARCHAR (MAX) NULL,
    [IsVerified] BIT           NOT NULL,
    CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED ([Task1Id] ASC),
    CONSTRAINT [FK_Task_Events] FOREIGN KEY ([EventId]) REFERENCES [Mast].[Events] ([EventId])
);

