CREATE TABLE [fin].[ChqInventory] (
    [chqInvId] INT          IDENTITY (1, 1) NOT NULL,
    [Brnhid]   INT          NOT NULL,
    [FrmChqno] NUMERIC (10) CONSTRAINT [DF_ChqInventory_FrmChqno] DEFAULT ((0)) NOT NULL,
    [Tochqno]  NUMERIC (18) NOT NULL,
    [Lastindx] NUMERIC (18) CONSTRAINT [DF_ChqInventory_Lastindx] DEFAULT ((0)) NOT NULL,
    [ISfinish] BIT          CONSTRAINT [DF_ChqInventory_ISfinish] DEFAULT ((0)) NOT NULL,
    [PostedBy] INT          NOT NULL,
    [PostedOn] DATETIME     NOT NULL,
    CONSTRAINT [PK_ChqInventory] PRIMARY KEY CLUSTERED ([chqInvId] ASC)
);

