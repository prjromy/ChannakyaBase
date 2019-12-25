CREATE function [dbo].[FGetHeadOffice]()
returns @temp table (
CompanyId int,BranchName varchar(200),Region varchar(200),State varchar(200),Address varchar(200),PhoneNo varchar(200),Email varchar(200),FaxNo varchar(100),Prefix varchar(50),ParentId int,IsBranch bit,AdditionalUser int,IPAddress varchar(20),TDate datetime
)
as




begin
declare @CompanyId int,@BranchName varchar(200),@Region varchar(200),@State varchar(200),@Address varchar(200),@PhoneNo varchar(200),@Email varchar(200),@FaxNo varchar(100),@Prefix varchar(50),@ParentId int,@IsBranch bit,@AdditionalUser int,@IPAddress varchar(20),@TDate datetime

set @CompanyId=0
set @BranchName=(select PValue from Lg.ParamValue where PId=26)
set @Region=(select PValue from Lg.ParamValue where PId=44)
set @State=(select PValue from Lg.ParamValue where PId=45)
set @Address=(select PValue from Lg.ParamValue where PId=5)
set @PhoneNo=(select PValue from Lg.ParamValue where PId=6)
set @Email=(select PValue from Lg.ParamValue where PId=7)
set @FaxNo=(select PValue from Lg.ParamValue where PId=32)
set @Prefix=(select PValue from Lg.ParamValue where PId=33)
set @AdditionalUser=(select PValue from Lg.ParamValue where PId=34)
set @IPAddress=(select PValue from Lg.ParamValue where PId=35)
set @TDate=(select PValue from lg.ParamValue where PId=47)


insert into @temp VALUES(@CompanyId,@BranchName,@Region,@State,@Address,@PhoneNo,@Email,@FaxNo,@Prefix,0,@IsBranch,@AdditionalUser ,@IPAddress,@TDate )

return

END