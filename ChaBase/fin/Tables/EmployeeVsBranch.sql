CREATE TABLE [fin].[EmployeeVsBranch] (
    [MapId]       INT      IDENTITY (1, 1) NOT NULL,
    [BranchId]    INT      NOT NULL,
    [EmpId]       INT      NOT NULL,
    [StartFrom]   DATETIME NOT NULL,
    [PostedBy]    INT      NOT NULL,
    [PostedOn]    DATETIME NOT NULL,
    [CurrentRole] INT      NOT NULL,
    CONSTRAINT [PK_EmployeeVsBranch] PRIMARY KEY CLUSTERED ([MapId] ASC)
);

