CREATE TABLE [LG].[BloodGroup] (
    [BGId]   INT            IDENTITY (1, 1) NOT NULL,
    [BGName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LG.BloodGroup] PRIMARY KEY CLUSTERED ([BGId] ASC)
);

