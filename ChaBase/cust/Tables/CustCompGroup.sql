CREATE TABLE [cust].[CustCompGroup] (
    [CCGroupID]   SMALLINT     IDENTITY (1, 1) NOT NULL,
    [CCGroupName] VARCHAR (30) NOT NULL,
    CONSTRAINT [PK_CustCompGroup] PRIMARY KEY CLUSTERED ([CCGroupID] ASC)
);

