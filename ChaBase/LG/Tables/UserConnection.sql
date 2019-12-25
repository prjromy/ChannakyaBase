CREATE TABLE [LG].[UserConnection] (
    [UserId]       INT            NOT NULL,
    [ConnectionID] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LG.UserConnection] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

