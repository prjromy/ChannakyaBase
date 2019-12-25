CREATE TABLE [LG].[Designation] (
    [DesignationId] INT            IDENTITY (1, 1) NOT NULL,
    [PDGId]         INT            NOT NULL,
    [DGName]        NVARCHAR (100) NOT NULL,
    [PostedBy]      INT            NOT NULL,
    [PostedOn]      DATETIME       NOT NULL,
    CONSTRAINT [PK_LG.Designation] PRIMARY KEY CLUSTERED ([DesignationId] ASC)
);

