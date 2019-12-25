CREATE TABLE [acc].[BatchDescription] (
    [BId]       INT          IDENTITY (1, 1) NOT NULL,
    [BatchName] VARCHAR (50) NOT NULL,
    [Prefix]    VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_BatchDescription] PRIMARY KEY CLUSTERED ([BId] ASC)
);

