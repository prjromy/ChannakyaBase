CREATE TABLE [fin].[SectorDef] (
    [CDepSector]    SMALLINT       IDENTITY (1, 1) NOT NULL,
    [CDepSectorNam] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_CustSector] PRIMARY KEY CLUSTERED ([CDepSector] ASC)
);

