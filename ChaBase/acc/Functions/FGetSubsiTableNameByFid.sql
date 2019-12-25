CREATE function acc.FGetSubsiTableNameByFid(@FID int)
returns @temp table (STBId int)
as
begin


insert into @temp select d.STBId from acc.FinAcc a inner join
 acc.FinSys2 b on a.F2Type=b.F2id inner join
  acc.FinSys1 c on b.F1id=c.F1id  inner join 
  acc.SubsiSetup d on c.F1Des=d.Title 
   where a.Fid=@FID
   if((select count(*) from @temp)=0)
    insert into @temp select a.STBId from acc.SubsiSetup a inner join acc.SubsiVSFId b on a.SSId=b.SSId inner join acc.SubsiTable c on a.STBId=c.STBId  where b.Fid=@FID

return
end