CREATE TABLE [acc].[CurrencyType] (
    [CTId]           INT           IDENTITY (1, 1) NOT NULL,
    [Country]        VARCHAR (50)  NOT NULL,
    [CurrencyName]   VARCHAR (100) NOT NULL,
    [CurrencyCode]   VARCHAR (50)  NOT NULL,
    [CurrencySymbol] VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_CurrencyType] PRIMARY KEY CLUSTERED ([CTId] ASC)
);

