CREATE TABLE [Mast].[Events] (
    [EventId]     INT           NOT NULL,
    [EventName]   VARCHAR (300) NULL,
    [Description] VARCHAR (MAX) NULL,
    [ModId]       TINYINT       NOT NULL,
    [EventType]   INT           NULL,
    CONSTRAINT [PK__Event__7944C810FF7AADFC] PRIMARY KEY CLUSTERED ([EventId] ASC),
    CONSTRAINT [FK_Events_Modules] FOREIGN KEY ([ModId]) REFERENCES [Mast].[Modules] ([Modid])
);

