
create PROCEDURE [dbo].[pset_UpdateAdjustmentLD]
	(
	   @Iaccno NUMERIC,
	   @PID INT,
	   @AMT MONEY
	)
AS
BEGIN
	
	SET NOCOUNT ON;
    DECLARE @sType bit
	declare @tDt  datetime
	declare @iPostDt datetime
	set @sType=(select sType from fin.ADetail inner join fin.ProductDetail on ADetail.PID=ProductDetail.PID inner join fin.SchmDetails on SchmDetails.SDID=ProductDetail.SDID where iaccno = @Iaccno)
	if @sType=1--Loan A/c
	begin	
		if @PID=0	--Int Income
			begin				
				
				set @tDt=(SELECT      DISTINCT(   branchGlobal.Tdate )	
				FROM      
                    	  	fin.ADetail  as adetail INNER JOIN
                      		dbo.BrnchGlobal as branchGlobal ON adetail.BrchID = branchGlobal.BrnchID where adetail.IAccno=@Iaccno)
				
				
				select @iPostDt=max(schDate) from fin.ALSch where IAccno=@Iaccno and schDate<=@tDt   

				update fin.ALSch set calcInt=ISNULL(calcInt,0)+@AMT where IAccno=@Iaccno and schDate = @iPostDt
				update fin.ALoan set IonPA=ISNULL(IonPA,0)+@AMT  where IAccno=@Iaccno				
			end		
		if @PID=1	--Int On Int
			begin				
				update fin.ALoan set IonIA=ISNULL(IonIA,0)+@AMT  where IAccno=@Iaccno
			end				
		if @PID=2	--Int On Charge
			begin				
				update fin.ALoan set IonCA=ISNULL(IonCA,0)+@AMT where IAccno=@Iaccno
			end				
		if @PID=3	--Pen On Prin
			begin				
				update fin.ALoan set PonPrA=ISNULL(PonPrA,0)+@AMT where IAccno=@Iaccno
			end
		if @PID=4	--Pen On Int
			begin				
				update fin.ALoan set PonIA=ISNULL(PonIA,0)+@AMT where IAccno=@Iaccno
			end	
	end


	
	if @stype=0 
	
	begin
	if  @pid=5
	begin
	execute  pset_updateinttocap @Iaccno
	
	
	end 
	end	
	


END