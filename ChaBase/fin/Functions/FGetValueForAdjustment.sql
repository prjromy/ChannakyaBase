CREATE Function [fin].[FGetValueForAdjustment](@IAccNo int, @Type tinyint)
returns money as
begin
declare @Value money
if(select stype from fin.ADetail ad inner join fin.ProductDetail pd on pd.pid = ad.pid inner join fin.SchmDetails sd on sd.SDID = pd.sdid where IAccno = @IAccNo) = 0 --deposit
begin
	set @Value = (select IonIA from fin.[FGetReportInterestDetails]() where IAccno  = @IAccNo	)
end
else --loan
begin
	if(@Type = 0) --interest accured
	begin
	set @Value = (select IonIA from fin.[FGetReportInterestDetails]() where IAccno  =@IAccNo	)
	end
	else if @Type = 1 --Interest on Interest
	begin
	set @Value = (select IonIR from fin.[FGetReportInterestDetails]() where IAccno  = @IAccNo	)
	end
	else if @Type=2--Interest on Charge
	begin
	set @Value=(select IonCA from fin.[FGetReportInterestDetails]() where IAccno  = @IAccNo	)
	end
	else if @Type=3--Penalty on Principal
	begin
	set @Value=(select PonPrR from fin.[FGetReportInterestDetails]() where IAccno=@IAccNo)
	end
	else if @Type=4--Penalty on Interest
	begin
	set @Value=(select PonIR from fin.[FGetReportInterestDetails]() where IAccno=@IAccNo)
	end

end

return @Value
end

--select * from fin.ADetail ad inner join fin.ProductDetail pd on pd.pid = ad.pid inner join fin.SchmDetails sd on sd.SDID = pd.sdid