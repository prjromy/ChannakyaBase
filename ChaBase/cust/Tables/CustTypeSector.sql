CREATE TABLE [cust].[CustTypeSector] (
    [CTSID]      INT      IDENTITY (1, 1) NOT NULL,
    [CTypeID]    TINYINT  NOT NULL,
    [CDepSector] SMALLINT NOT NULL,
    PRIMARY KEY CLUSTERED ([CTSID] ASC),
    CONSTRAINT [FK_CustTypeSector_CustType] FOREIGN KEY ([CTypeID]) REFERENCES [cust].[CustType] ([CtypeID]),
    CONSTRAINT [FK_CustTypeSector_SectorDef] FOREIGN KEY ([CDepSector]) REFERENCES [fin].[SectorDef] ([CDepSector])
);

