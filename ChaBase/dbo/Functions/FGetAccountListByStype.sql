create function [dbo].[FGetAccountListByStype](@Stype int)
returns table 
as
return
		(
			select
				a.*,s.SDID,s.SDName,p.PName from fin.ADetail a
			inner join 
				fin.ProductDetail p
				on p.PID=a.PID
			inner join 
				fin.SchmDetails s 
				on s.SDID=p.SDID 
			where
				s.SType=@Stype
		)