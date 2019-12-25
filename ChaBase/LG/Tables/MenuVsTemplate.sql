CREATE TABLE [LG].[MenuVsTemplate] (
    [MenuId]            INT NOT NULL,
    [TemplateId]        INT NOT NULL,
    [MenuTemplate_MTId] INT NULL,
    CONSTRAINT [PK_LG.MenuVsTemplate] PRIMARY KEY CLUSTERED ([MenuId] ASC, [TemplateId] ASC)
);

