CREATE TABLE [LG].[Gender] (
    [GenderId]   INT            IDENTITY (1, 1) NOT NULL,
    [GenderName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LG.Gender] PRIMARY KEY CLUSTERED ([GenderId] ASC)
);

