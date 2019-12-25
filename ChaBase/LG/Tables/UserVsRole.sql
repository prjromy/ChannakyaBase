CREATE TABLE [LG].[UserVsRole] (
    [URId]    INT      IDENTITY (1, 1) NOT NULL,
    [RoleId]  INT      NOT NULL,
    [EffFrom] DATETIME NULL,
    [EffTo]   DATETIME NULL,
    [Status]  BIT      NOT NULL
);

