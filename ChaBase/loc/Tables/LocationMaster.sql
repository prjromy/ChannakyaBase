CREATE TABLE [loc].[LocationMaster] (
    [LId]    INT     NOT NULL,
    [MaxLvl] TINYINT CONSTRAINT [DF_LocationMaster_MaxLvl] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LocationMaster] PRIMARY KEY CLUSTERED ([LId] ASC),
    CONSTRAINT [FK_LocationMaster_Location] FOREIGN KEY ([LId]) REFERENCES [loc].[Location] ([LId])
);

