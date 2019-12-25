CREATE TABLE [LG].[MenuTemplate] (
    [MenuTemplateId] INT            IDENTITY (1, 1) NOT NULL,
    [MTName]         NVARCHAR (100) NOT NULL,
    [DesignationId]  INT            NOT NULL,
    [PostedBy]       INT            NOT NULL,
    [PostedOn]       DATETIME       NOT NULL,
    CONSTRAINT [PK_LG.MenuTemplate] PRIMARY KEY CLUSTERED ([MenuTemplateId] ASC)
);

