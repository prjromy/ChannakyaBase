CREATE FUNCTION [fin].[FGetReportCashFlow] (@Fdate SMALLDATETIME, @Tdate SMALLDATETIME, @Brnhid INT,@userId int,@currId int)

RETURNS TABLE ---MARKED ENCRYPTION  as

 return (

SELECT Brhid
            ,cf.FUserid as UserId1
			,cf.UserId as UserId2
						
			,Tno

			,TDate

			,cfd.TEXT +case when @userId=FUserid then ' :--Send To :- '  + user1.UserName  else ' :--Send By :- '  +usr.UserName end AS Tdesc

			,TType

			,Currid,
			case when @userId=cf.UserID then Dramt else 0 end 
			as Dramt,
			case when @userId=FUserid then Dramt else 0 end
			as Cramt
			,tt.TtypeDesc as Remarks,
			Vno
		FROM fin.CashFlow AS cf

		INNER JOIN fin.FGetCashFlowDictionary() cfd ON cf.TType = cfd.Id

		INNER JOIN lg.[User] usr ON usr.UserId = cf.FUserid
		inner join lg.[User] user1 on user1.UserId=cf.UserID
		inner join trans.ttype tt on tt.id=cf.status
		WHERE ttype IN (

				1

				,2

				,3

				,4

				,5

				,6

				)

			
		 and (FUserid is not null) and Brhid = @Brnhid and tdate between  @fdate and @tdate  and (FUserid=@userId or cf.UserId=@userId)and Currid=@currId

		

		

		

)