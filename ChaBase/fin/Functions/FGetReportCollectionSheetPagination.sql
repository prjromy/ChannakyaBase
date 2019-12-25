Create function [fin].[FGetReportCollectionSheetPagination](@FDate datetime,@TDate datetime,@branchId int,@PageNo int,@PageSize int)
returns @temp table (collSheetId bigint,TDate datetime, SheetNo int,CollectorId int,UserId int, EmployeeName nvarchar, Amount money,LoanAmount money,DepositAmount money,
LoanCount int,DepositCount int,totalCount int)  
as 
begin

declare @count int = 0
declare @collSheetId bigint,@TDate2 datetime, @SheetNo int,@CollectorId int,@UserId int, @EmployeeName nvarchar, @Amount money,@LoanAmount money,@DepositAmount money,
@LoanCount int,@DepositCount int

select @count = count(EM.UserId) FROM FIN.collsheettrans CT inner join fin.CollSheet CM on CM.CollSheetId = CT.CollSheetId
left join [fin].[FGetAllUsersWithDesignation]() EM on EM.UserId=CM.CollectorId
where ApprovedBy is not null and CM.TDate between @FDate and @TDate and cm.BrchId=@branchId
group by CM.CollSheetId, cm.TDate, cm.SheetNo, EM.EmployeeName ,CM.CollectorId,EM.UserId


 Declare V2 cursor for
 select CM.collSheetId,CM.TDate, CM.SheetNo,CM.CollectorId,EM.UserId, EM.EmployeeName,sum(CT.Amount) as [TotalAmount], 
[LoanAmount] = (SELECT SUM(AMOUNT) FROM fin.collsheettrans T WHERE T.CollSheetId = CM.CollSheetId AND SType=1),
[DepositAmount] = (SELECT SUM(AMOUNT) FROM fin.collsheettrans T WHERE T.CollSheetId = CM.CollSheetId AND SType=0),
[LoanCount] = (SELECT count(*) FROM fin.collsheettrans T WHERE T.CollSheetId = CM.CollSheetId AND SType=1),
[DepositCount] = (SELECT count(*) FROM fin.collsheettrans T WHERE T.CollSheetId = CM.CollSheetId AND SType=0)
FROM FIN.collsheettrans CT 
inner join fin.CollSheet CM on CM.CollSheetId = CT.CollSheetId
left join [fin].[FGetAllUsersWithDesignation]() EM on EM.UserId=CM.CollectorId
where ApprovedBy is not null and CM.TDate between @FDate and @TDate and cm.BrchId=@branchId
group by CM.CollSheetId, cm.TDate, cm.SheetNo, EM.EmployeeName ,CM.CollectorId,EM.UserId
   ORDER BY EM.UserId desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY
 
 OPEN V2 
	FETCH FROM V2 INTO  @collSheetId ,@TDate2 , @SheetNo ,@CollectorId ,@UserId , @EmployeeName , @Amount ,@LoanAmount ,@DepositAmount ,@LoanCount ,@DepositCount 
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values( @collSheetId ,@TDate2 , @SheetNo ,@CollectorId ,@UserId , @EmployeeName , @Amount ,@LoanAmount ,@DepositAmount ,@LoanCount ,@DepositCount,@count )
	FETCH NEXT FROM V2 INTO @collSheetId ,@TDate2 , @SheetNo ,@CollectorId ,@UserId , @EmployeeName , @Amount ,@LoanAmount ,@DepositAmount ,@LoanCount ,@DepositCount 
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end