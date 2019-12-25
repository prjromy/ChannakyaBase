CREATE TABLE [LG].[Employees] (
    [EmployeeId]     INT             IDENTITY (1, 1) NOT NULL,
    [EmployeeNo]     NVARCHAR (MAX)  NOT NULL,
    [EmployeeName]   NVARCHAR (100)  NOT NULL,
    [DGId]           INT             NOT NULL,
    [DeptId]         INT             NOT NULL,
    [Religion]       INT             NOT NULL,
    [Nationality]    INT             NOT NULL,
    [MaritialStatus] SMALLINT        NOT NULL,
    [DateOfJoin]     DATETIME2 (7)   NOT NULL,
    [Gender]         SMALLINT        NOT NULL,
    [BloodGroup]     SMALLINT        NOT NULL,
    [Photo]          VARBINARY (MAX) NULL,
    [Initials]       INT             NOT NULL,
    [PostedOn]       DATETIME        NOT NULL,
    [PostedBy]       INT             NOT NULL,
    [ModifiedBy]     INT             NULL,
    [ModifiedOn]     DATETIME        NULL,
    [Status]         SMALLINT        NOT NULL,
    [BranchId]       INT             NOT NULL,
    CONSTRAINT [PK_LG.Employees] PRIMARY KEY CLUSTERED ([EmployeeId] ASC)
);

