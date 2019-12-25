CREATE TABLE [LG].[MaritialStatus] (
    [MSId]   INT            IDENTITY (1, 1) NOT NULL,
    [MSName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LG.MaritialStatus] PRIMARY KEY CLUSTERED ([MSId] ASC)
);

