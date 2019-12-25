CREATE TABLE [LG].[MenuMain] (
    [MenuId]        INT             IDENTITY (1, 1) NOT NULL,
    [MenuCaption]   NVARCHAR (100)  NOT NULL,
    [PMenuId]       INT             NOT NULL,
    [Image]         VARBINARY (MAX) NULL,
    [MenuOrder]     INT             NOT NULL,
    [IsGroup]       BIT             NOT NULL,
    [IsEnable]      BIT             NOT NULL,
    [IsContextMenu] BIT             NOT NULL,
    [Controler]     VARCHAR (200)   NULL,
    [Acton]         VARCHAR (200)   NULL,
    CONSTRAINT [PK_LG.MenuMain] PRIMARY KEY CLUSTERED ([MenuId] ASC)
);

