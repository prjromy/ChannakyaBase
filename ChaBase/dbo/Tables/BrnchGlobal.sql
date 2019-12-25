CREATE TABLE [dbo].[BrnchGlobal] (
    [BrnchID]    INT           NOT NULL,
    [CalSID]     TINYINT       NOT NULL,
    [CalCID]     TINYINT       NOT NULL,
    [Tdate]      SMALLDATETIME NOT NULL,
    [UseLimit]   BIT           NOT NULL,
    [fisYear]    INT           NULL,
    [VnoExt]     NUMERIC (18)  NULL,
    [atclosing]  BIT           NULL,
    [MigTdate]   SMALLDATETIME NULL,
    [CleCode]    NVARCHAR (50) NULL,
    [intExpMod]  TINYINT       NULL,
    [Floint]     MONEY         NULL,
    [IsCalcIOnI] BIT           NULL
);

