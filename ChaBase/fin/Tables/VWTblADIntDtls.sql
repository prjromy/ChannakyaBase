CREATE TABLE [fin].[VWTblADIntDtls]
(
	[Iaccno] [numeric](18, 0) NOT NULL,
	[ICBID] [tinyint] NULL,
	[ICB] [nvarchar](50) NULL,
	[intCalpSchmID] [tinyint] NULL,
	[intCalpSchm] [nvarchar](50) NULL,
	[MID] [tinyint] NULL,
	[NIAccNo] [nvarchar](50) NULL,
	[TaxRate] [float] NULL,
	[MatDate] [datetime] NULL,
	[IntToExp] [money] NULL,
	[IntToCap] [money] NULL,
	[BrchId] [tinyint] NULL
)
