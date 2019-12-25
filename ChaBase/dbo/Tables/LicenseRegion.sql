CREATE TABLE [dbo].[LicenseRegion] (
    [RegID]     SMALLINT     IDENTITY (1, 1) NOT NULL,
    [RegionNam] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_LicenseRegion] PRIMARY KEY CLUSTERED ([RegID] ASC)
);

