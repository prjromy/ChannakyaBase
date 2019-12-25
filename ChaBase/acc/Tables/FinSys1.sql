CREATE TABLE [acc].[FinSys1] (
    [F1id]     INT           NOT NULL,
    [F1Des]    VARCHAR (50)  NULL,
    [IsGroup]  BIT           NOT NULL,
    [IsEnable] BIT           NOT NULL,
    [Note]     VARCHAR (500) NULL,
    CONSTRAINT [PK__FinSys1__9A390C6CB68F6382] PRIMARY KEY CLUSTERED ([F1id] ASC)
);

