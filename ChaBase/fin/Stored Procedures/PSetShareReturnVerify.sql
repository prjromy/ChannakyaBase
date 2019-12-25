
CREATE PROCEDURE [fin].[PSetShareReturnVerify]
(
	@TNO NUMERIC OUTPUT
	,@ApprovedBy int	
)
---MARKED ENCRYPTION  
AS
Begin
	set nocount on;
	declare @trancount int;
	declare @REGNO		NUMERIC
	declare @SID	    NUMERIC
	declare @SOLDTO	    NUMERIC = NULL
	declare @SDATE		SMALLDATETIME
	declare @SQTY		NUMERIC
	declare @NOTE		NVARCHAR(500)
	declare @POSTEDBY	INT
	declare @TTYPE		INT
	declare @STYPE		INT
	declare @BRCHID		INT
	declare @VNO		INT = NULL

	select @REGNO =REGNO		
	,@SID		=SCrtNo	
	,@SOLDTO	=SOLDTO		
	,@SDATE		=SDATE		
	,@SQTY		=SQTY		
	,@NOTE		=NOTE		
	,@POSTEDBY	=POSTEDBY	
	,@TTYPE		=TTYPE		
	,@STYPE		=STYPE		
	,@BRCHID	=BRCHID		
	,@VNO		= VNO	from fin.ShrsReturn	WHERE Tno=@TNO
--

--select *from fin.ShrsReturn	WHERE Tno=416124
	declare @NTSno numeric
	Declare @NFSno numeric
	Declare @Scrtno numeric
	Declare @NSid numeric
	Declare @Nnote nvarchar(500)
	Declare @NQty numeric
	Declare @FSno numeric
	Declare @TSno numeric
	Declare @TQty numeric
	Declare @SRate money

	Set @SRate	= (Select ShrRate From Mast.ShrSetup)
	Set @FSno	= (Select FSno from fin.SCrtDtls where Sid=@SID)
	Set @TSno	= (Select TSno from fin.SCrtDtls where Sid=@SID)
	Set @TQty	= (@TSno-@FSno+1)

	set @NTSno	= @FSno + @SQty - 1
	set @NFSno	= @NTSno + 1
	Set @NQty	= @TSno - @FSno - @SQty  + 1


	Declare @IsBank bit
	Set @IsBank	= (select convert(bit,PValue) from lg.ParamValue where PId=6004)

	set @trancount = @@trancount;
	begin try
	if @trancount = 0
		begin transaction
	else
		save transaction [fin.PSetShareReturnVerify];


	If @IsBank =0
	Begin 
		If @SQty < @TQty
		Begin		
			Set @Nnote = '[Share Broken Down For Partial Transfer]' + @note
			Insert into fin.ShrReturn(RegNo,SCrtNo,Tno,Sdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype) 
				values( @RegNo,@Sid,@Tno,@Sdate,@TQty,@TQty*@SRate,@Nnote,@PostedBy,@ApprovedBy,2)
			update fin.SCrtDtls set Status=0 where Sid=@Sid

			set @Scrtno = (select isnull(max(Scrtno)+1,(select CrtNo from Mast.Shrsetup))from fin.SCrtDtls)
			insert into fin.SCrtDtls (SCrtno,Fsno,TSno,SQty,Status,Stype) Values (@Scrtno,@FSno,@NTSno,@SQty,1,@Stype)
		
		
			set @NSid = (select  @@identity)
			Set @Nnote = 'Share Range From '+ cast(@FSno as nvarchar(10)) + ' To ' + Cast(@TSno as nvarchar(10)) + ' Broken '
			insert into fin.ShrPurchase (Regno,Scrtno,Tno,Pdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype,brchid,VNo) 
				values (@RegNo, @NSid,@tno,@Sdate,@SQty,@SQty*@SRate,@Nnote, @PostedBy,@ApprovedBy,2,@BRCHID,@VNo)
			insert into fin.ShrReturn(RegNo,SCrtNo,Tno,Sdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype,BrchID,VNo) 
				values( @RegNo,@NSid,@Tno,@Sdate,@SQty,@SQty*@SRate,@Note,@PostedBy,@ApprovedBy,@Ttype,@BrchID,@VNo)
			Update fin.SCrtDtls set Status=0 where Sid=@NSid
		
			set @Scrtno = @Scrtno + 1
			insert into fin.SCrtDtls (SCrtno,Fsno,TSno,SQty,Status,Stype) Values (@Scrtno,@NFSno,@TSno,@NQty,1,@Stype)	
		
			set @NSid = (select  @@identity)
			insert into fin.ShrPurchase (Regno,Scrtno,Tno,Pdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype,brchid,VNo) 
				values (@RegNo,@NSid,@tno,@Sdate,@NQty,@NQty*@SRate,@Nnote, @PostedBy,@ApprovedBy,2,@BRCHID,@VNo)
		
		End
		Else
		Begin
			Insert into fin.ShrReturn(RegNo,SCrtNo,Tno,Sdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype,BrchID) 
				values( @RegNo,@Sid,@Tno,@Sdate,@TQty,@TQty*@SRate,@Note,@PostedBy,@ApprovedBy,@Ttype,@BrchID)
			update fin.SCrtDtls set Status=0 where Sid=@Sid
		End	
	End

	If @IsBank = 1
	Begin
		If @SoldTo is not null
		Begin	
			If @SQty < @TQty
			Begin		
				Set @Nnote = '[Share Broken Down For Partial Transfer]' + @note
				Insert into fin.ShrReturn(RegNo,SCrtNo,SoldTo,Tno,Sdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype) 
					values( @RegNo,@Sid,@RegNo,@Tno,@Sdate,@TQty,@TQty*@SRate,@Nnote,@PostedBy,@ApprovedBy,2)
				update fin.SCrtDtls set Status=0 where Sid=@Sid
		
				set @Scrtno = (select isnull(max(Scrtno)+1,(select CrtNo from mast.Shrsetup))from fin.ScrtDtls)
				insert into fin.SCrtDtls (SCrtno,Fsno,TSno,SQty,Status,Stype) Values (@Scrtno,@FSno,@NTSno,@SQty,1,@Stype)
				
			
				set @NSid = (select  @@identity)
				Set @Nnote = 'Share Range From '+ cast(@FSno as nvarchar(10)) + ' To ' + Cast(@TSno as nvarchar(10)) + ' Broken '
				insert into fin.ShrPurchase (Regno,Scrtno,Tno,Pdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype) 
					values (@RegNo, @NSid,@tno,@Sdate,@SQty,@SQty*@SRate,@Nnote, @PostedBy,@ApprovedBy,2)
				insert into fin.ShrReturn(RegNo,SCrtNo,SoldTo,Tno,Sdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype) 
					values( @RegNo,@NSid,@SoldTo,@Tno,@Sdate,@SQty,@SQty*@SRate,@Note,@PostedBy,@ApprovedBy,@Ttype)
				Update fin.SCrtDtls set Status=0 where Sid=@NSid
				
				set @Scrtno = @Scrtno + 1
				insert into fin.SCrtDtls (SCrtno,Fsno,TSno,SQty,Status,Stype) Values (@Scrtno,@NFSno,@TSno,@NQty,1,@Stype)	
				
				set @NSid = (select  @@identity)
				insert into fin.ShrPurchase (Regno,Scrtno,Tno,Pdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype) 
					values (@RegNo, @NSid,@tno,@Sdate,@NQty,@NQty*@SRate,@Nnote, @PostedBy,@ApprovedBy,2)
			End
			Else
			Begin
				Insert into fin.ShrReturn(RegNo,SCrtNo,SoldTo,Tno,Sdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype) 
					values( @RegNo,@Sid,@SoldTo,@Tno,@Sdate,@TQty,@TQty*@SRate,@Note,@PostedBy,@ApprovedBy,@Ttype)
			End
		End
	End
	DELETE FROM fin.ShrSReturn WHERE tno = @tno
	lbexit:
	if @trancount = 0
		commit;
		return;
	end try
	begin catch
		declare @error int, @message varchar(4000), @xstate int;
		select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
		if @xstate = -1
			rollback;
		if @xstate = 1 and @trancount = 0
			rollback
		if @xstate = 1 and @trancount > 0
			rollback transaction usp_my_procedure_name;
		raiserror (' [fin.PSetShareReturnVerify]: %d: %s', 16, 1, @error, @message) ;
	end catch
end