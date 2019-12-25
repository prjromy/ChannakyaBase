CREATE TABLE [LG].[UserVSBranch] (
    [Id]       INT      IDENTITY (1, 1) NOT NULL,
    [UserId]   INT      NOT NULL,
    [BranchId] INT      NOT NULL,
    [From]     DATETIME NOT NULL,
    [To]       DATETIME NULL,
    [IsEnable] BIT      NOT NULL,
    [RoleId]   INT      NOT NULL,
    [PostedBy] INT      NOT NULL,
    [PostedOn] DATETIME NOT NULL,
    CONSTRAINT [PK_dbo.UserVSBranch] PRIMARY KEY CLUSTERED ([Id] ASC)
);

