CREATE TABLE [LG].[ParamValue] (
    [PId]          INT            NOT NULL,
    [DTId]         TINYINT        NOT NULL,
    [PDescription] NVARCHAR (MAX) NOT NULL,
    [PValue]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LG.ParamValue] PRIMARY KEY CLUSTERED ([PId] ASC)
);

