Create function mig.ProductAndDuration()
returns table as return
Select DurationName= case when Dur< 1 then  convert(varchar, Dur * 1000)  + ' Days' else convert(varchar,Dur) + ' Month(s)' end, Value = case when Dur< 1 then Dur * 1000 else Dur * 30 end,dur as OldDuration from (

select distinct Dur from nimbus.dbo.ADur ) x where x.Dur is not null
union

Select DurationName= case when Duration< 1 then  convert(varchar, duration * 1000)  + ' Days' else convert(varchar,Duration) + ' Month(s)' end, Value = case when Duration< 1 then duration * 1000 else Duration * 30 end,duration as OldDuration from (
select distinct Duration from nimbus.dbo.ProductDetail ) x where x.Duration is not null