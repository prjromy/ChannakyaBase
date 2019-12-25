


CREATE FUNCTION [fin].[FCRTMTHENDDT]()
RETURNS  @temp TABLE( YRID INT NOT NULL,MNTH INT NOT NULL,DYS INT NOT NULL,MTHEND SMALLDATETIME,ME TINYINT,QE TINYINT,SE TINYINT,YE TINYINT) 
---MARKED ENCRYPTION 
As
Begin
	--declare @Nyr int,
	--@ME1 SMALLDATETIME, @M1 INT,
	--@ME2 SMALLDATETIME, @M2 INT,
	--@ME3 SMALLDATETIME, @M3 INT,
	--@ME4 SMALLDATETIME, @M4 INT,
	--@ME5 SMALLDATETIME, @M5 INT,
	--@ME6 SMALLDATETIME, @M6 INT,
	--@ME7 SMALLDATETIME, @M7 INT,
	--@ME8 SMALLDATETIME, @M8 INT,
	--@ME9 SMALLDATETIME, @M9 INT,
	--@ME10 SMALLDATETIME, @M10 INT,
	--@ME11 SMALLDATETIME, @M11 INT,
	--@ME12 SMALLDATETIME, @M12 INT

	--DECLARE Cur cursor for
	--select n.NYear, dbo.FGETDATEAD(n.NYear,1,n.M1) ME1, M1, dbo.FGETDATEAD(n.NYear,2,n.M2) ME2, M2, dbo.FGETDATEAD(n.NYear,3,n.M3) ME3, M3, dbo.FGETDATEAD(n.NYear,4,n.M4) ME4,M4,
	--dbo.FGETDATEAD(n.NYear,5,n.M5) ME5, M5, dbo.FGETDATEAD(n.NYear,6,n.M6) ME6,M6,dbo.FGETDATEAD(n.NYear,7,n.M7) ME7,M7,dbo.FGETDATEAD(n.NYear,8,n.M8) ME8,M8,
	--dbo.FGETDATEAD(n.NYear,9,n.M9) ME9,M9,dbo.FGETDATEAD(n.NYear,10,n.M10) ME10,M10,dbo.FGETDATEAD(n.NYear,11,n.M11) ME11,M11,dbo.FGETDATEAD(n.NYear,12,n.M12) ME12,M12
	--from NDateD n 
	--where n.NYear>= year((select tdate from BrnchGlobal   where BrnchID =1))

	--open cur
	--FETCH FROM CUR INTO @Nyr, @ME1,@M1,@ME2,@M2,@ME3,@M3,@ME4,@M4,@ME5,@M5,@ME6,@M6,@ME7,@M7,@ME8,@M8,@ME9,@M9,@ME10,@M10,@ME11,@M11,@ME12,@M12
	--WHILE @@FETCH_STATUS=0
	--BEGIN
	--INSERT INTO @DtTbl values (@Nyr,1,@M1,@ME1,4,0,0,0)
	--INSERT INTO @DtTbl values (@Nyr,2,@M2,@ME2,4,0,0,0)
	--INSERT INTO @DtTbl values (@Nyr,3,@M3,@ME3,4,1,0,0)
	--INSERT INTO @DtTbl values (@Nyr,4,@M4,@ME4,4,0,0,0)
	--INSERT INTO @DtTbl values (@Nyr,5,@M5,@ME5,4,0,0,0)
	--INSERT INTO @DtTbl values (@Nyr,6,@M6,@ME6,4,1,2,0)
	--INSERT INTO @DtTbl values (@Nyr,7,@M7,@ME7,4,0,0,0)
	--INSERT INTO @DtTbl values (@Nyr,8,@M8,@ME8,4,0,0,0)
	--INSERT INTO @DtTbl values (@Nyr,9,@M9,@ME9,4,1,0,0)
	--INSERT INTO @DtTbl values (@Nyr,10,@M10,@ME10,4,0,0,0)
	--INSERT INTO @DtTbl values (@Nyr,11,@M11,@ME11,4,0,0,0)
	--INSERT INTO @DtTbl values (@Nyr,12,@M12,@ME12,4,1,2,3)	

	--FETCH NEXT FROM CUR INTO @Nyr, @ME1,@M1,@ME2,@M2,@ME3,@M3,@ME4,@M4,@ME5,@M5,@ME6,@M6,@ME7,@M7,@ME8,@M8,@ME9,@M9,@ME10,@M10,@ME11,@M11,@ME12,@M12
	--END

	--close cur
	--deallocate cur	
	
	--*********************************************************************************************************************************--
	--*********************************************************************************************************************************--
	--*************************************************updated by bikash on 12/25/2018*************************************************--
	--*********************************************************************************************************************************--
	--*********************************************************************************************************************************--

	declare @nYear int 
	set @nYear = (select dbo.FGetBSYear(dbo.FGetDateBS((select tdate from [fin].[LicenseBranch] where BrnhId =1))))
	
	insert into @temp
	
	select top 240 yrid, mnth, [days], dbo.fgetDateAD(yrid, mnth, [days]) as MTHEND, ME, QE, SE, YE from (
	select nYear [YRID], 1 [MNTH], M1 [Days], 4 ME, 0 QE, 0 SE, 0 YE from NDateD union all
	select nYear [YRID], 2 [MNTH], M2 [Days], 4 ME, 0 QE, 0 SE, 0 YE from NDateD union all
	select nYear [YRID], 3 [MNTH], M3 [Days], 4 ME, 1 QE, 0 SE, 0 YE from NDateD union all

	select nYear [YRID], 4 [MNTH], M4 [Days], 4 ME, 0 QE, 0 SE, 0 YE from NDateD union all
	select nYear [YRID], 5 [MNTH], M5 [Days], 4 ME, 0 QE, 0 SE, 0 YE from NDateD union all
	select nYear [YRID], 6 [MNTH], M6 [Days], 4 ME, 1 QE, 2 SE, 0 YE from NDateD union all

	select nYear [YRID], 7 [MNTH], M7 [Days], 4 ME, 0 QE, 0 SE, 0 YE from NDateD union all
	select nYear [YRID], 8 [MNTH], M8 [Days], 4 ME, 0 QE, 0 SE, 0 YE from NDateD union all
	select nYear [YRID], 9 [MNTH], M9 [Days], 4 ME, 1 QE, 0 SE, 0 YE from NDateD union all

	select nYear [YRID], 10 [MNTH], M10 [Days], 4 ME, 0 QE, 0 SE, 0 YE from NDateD union all
	select nYear [YRID], 11 [MNTH], M11 [Days], 4 ME, 0 QE, 0 SE, 0 YE from NDateD union all
	select nYear [YRID], 12 [MNTH], M11 [Days], 4 ME, 1 QE, 2 SE, 3 YE from NDateD ) x  where yrid >= @nYear order by yrid, mnth
	return 
return
end