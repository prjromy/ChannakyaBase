CREATE TABLE [LG].[User] (
    [UserId]               INT            IDENTITY (1, 1) NOT NULL,
    [MTId]                 INT            NOT NULL,
    [EmployeeId]           INT            NULL,
    [EffDate]              DATETIME       NULL,
    [TillDate]             DATETIME       NULL,
    [IsUnlimited]          BIT            NOT NULL,
    [IsActive]             BIT            NOT NULL,
    [Image]                TINYINT        NULL,
    [Email]                NVARCHAR (MAX) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LG.User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

