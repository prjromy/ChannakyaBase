CREATE TABLE [LG].[Status] (
    [StatusId]   INT            IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Status] PRIMARY KEY CLUSTERED ([StatusId] ASC)
);

