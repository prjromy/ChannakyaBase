CREATE TABLE [Mast].[taskDetails1] (
    [Task2Id]    INT      IDENTITY (1, 1) NOT NULL,
    [Task1Id]    INT      NOT NULL,
    [TaskTo]     INT      NOT NULL,
    [VerifiedOn] DATETIME NULL,
    [SeenOn]     DATETIME NULL,
    [RejectedOn] DATETIME NULL
);

