CREATE function [acc].[GetBatchListByVoucherNo](@userID int,@type int)
returns table as return(
select distinct BD.BId,BD.BatchName,BD.Prefix from [acc].[UserVSVoucher] as ac join 
             [acc].BatchDescription as BD on ac.BatchId=BD.BId  join acc.VoucherNo as VN on VN.BId=BD.BId  where ac.UserId=@userID and ac.VTypeId=@type

)