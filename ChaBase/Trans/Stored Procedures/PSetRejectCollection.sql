CREATE procedure [Trans].[PSetRejectCollection] @SheetNo int, @RejectedBy int,@RejectRemarks varchar(200)
as
begin
    set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Trans.PSetRejectCollection];

        declare @RetId int
		  declare @CollSheetId int=(select top 1 collsheetid from fin.CollSheet where SheetNo=@SheetNo)
			insert into fin.CollMainTemp
			select top 1 BrchId,TotalAmount as CollectionAmt,CollectorId,TDate,VDate,PostedBy,c.TotalAmt,  @SheetNo as SheetNo,@RejectedBy as RejectedBy,1 as Status,@RejectRemarks as RejectedRemarks from fin.CollSheet a inner join fin.CollSheetTrans b on a.CollSheetId=b.CollSheetId 
			inner join (select CollSheetId,Sum(amount) as TotalAmt from fin.CollSheetTrans where CollSheetId=@CollSheetId group by CollSheetId ) c on a.CollSheetId=b.CollSheetId where b.CollSheetId=@CollSheetId
			set @RetId=	cast((select @@IDENTITY) as int)
			insert into fin.CollTemp 
			select @RetId,IAccNo,[fin].[FGetScalarFidByIAccno](IAccNo) as Fid,[fin].[FGetStypeIAccno](IAccNo) as SType,Amount,Description from fin.CollSheetTrans where CollSheetId=@CollSheetId

			delete from fin.CollSheetTrans where CollSheetId=@CollSheetId
			delete from fin.CollSheet where CollSheetId=@CollSheetId
			

lbexit:
        if @trancount = 0
            commit;
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

        raiserror ('[Trans.PSetRejectCollection]: %d: %s', 16, 1, @error, @message) ;
    end catch
end