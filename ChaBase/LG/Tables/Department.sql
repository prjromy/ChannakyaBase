CREATE TABLE [LG].[Department] (
    [DepartmentId] INT            IDENTITY (1, 1) NOT NULL,
    [DeptName]     NVARCHAR (100) NOT NULL,
    [PDeptId]      INT            NOT NULL,
    [PostedBy]     INT            NOT NULL,
    [PostedOn]     DATETIME       NOT NULL,
    CONSTRAINT [PK_LG.Department] PRIMARY KEY CLUSTERED ([DepartmentId] ASC)
);

