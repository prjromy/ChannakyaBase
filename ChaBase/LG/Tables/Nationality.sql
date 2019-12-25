CREATE TABLE [LG].[Nationality] (
    [NId]   INT            IDENTITY (1, 1) NOT NULL,
    [NName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LG.Nationality] PRIMARY KEY CLUSTERED ([NId] ASC)
);

