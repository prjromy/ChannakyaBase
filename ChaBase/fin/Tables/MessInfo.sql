CREATE TABLE [fin].[MessInfo] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [EventValue] INT            NOT NULL,
    [Idtype]     TINYINT        NOT NULL,
    [Eventid]    TINYINT        NOT NULL,
    [Fdate]      SMALLDATETIME  NOT NULL,
    [Tdate]      SMALLDATETIME  NOT NULL,
    [Mdesc]      NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_MessInfo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

