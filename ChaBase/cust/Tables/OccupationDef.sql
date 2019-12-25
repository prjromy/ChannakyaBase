CREATE TABLE [cust].[OccupationDef] (
    [Occpn]      SMALLINT      IDENTITY (1, 1) NOT NULL,
    [occupation] NVARCHAR (25) NOT NULL,
    CONSTRAINT [PK_CustOccupation] PRIMARY KEY CLUSTERED ([Occpn] ASC)
);

