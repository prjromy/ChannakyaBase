CREATE PROCEDURE [dbo].[PGetInterestTOCapitalizeAccount]( @TDATE SMALLDATETIME, @BRNCHID INT) 
---MARKED ENCRYPTION  
As
	select ad.IAccno,ad.Accno,ad.Aname,pd.PName,SDName,intCalpSchm,ad.IRate,ad.PID,sd.SDID,intCalpSchmID,ac.ICBID  
	into #FGetADINTDTLS 
		from [fin].[FGetReportAccountInterestDetails]() ac 
		inner join 	fin.adetail ad on ad.IAccno=ac.IAccno
		inner join fin.ProductDetail pd on pd.pid=ad.PID
		inner join fin.SchmDetails sd on sd.SDID=pd.SDID
	where ad.AccState <> 2 and ad.AccState <> 6 and ad.BrchID=coalesce(@BRNCHID,ad.BrchID)

	select * into #FCrtMthEndDt from  fin.FCrtMthEndDt() where mthend = @Tdate

	select * from #FGetADINTDTLS where ICBID=2 and intcalpschmid in 

		(select ME from #FCrtMthEndDt where mthend =@tdate and ME>0

		union all

		select QE from #FCrtMthEndDt where mthend =@tdate and QE>0

		union all

		select SE from #FCrtMthEndDt where mthend =@tdate and SE>0

		union all

		select YE from #FCrtMthEndDt where mthend =@tdate and YE>0)

		union all

		select f.* from #FGetADINTDTLS f inner join fin.ADSch a on f.iaccno=a.iaccno where a.MDate =@tdate