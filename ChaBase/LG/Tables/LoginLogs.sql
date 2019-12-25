CREATE TABLE [LG].[LoginLogs] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserId]   INT            NOT NULL,
    [BranchId] INT            NOT NULL,
    [RoleId]   INT            NOT NULL,
    [From]     DATETIME       NOT NULL,
    [To]       DATETIME       NULL,
    [IP]       NVARCHAR (200) NULL,
    CONSTRAINT [PK_dbo.LoginLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

