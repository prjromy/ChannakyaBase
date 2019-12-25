CREATE TABLE [fin].[ChequeBookStatus] (
    [Id]           TINYINT       NOT NULL,
    [ChequeStatus] VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_ChequeBookStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

