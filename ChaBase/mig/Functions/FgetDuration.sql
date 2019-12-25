CREATE function mig.FgetDuration()



returns table as return



select  d.*,pd.DurationName,pd.OldDuration from fin.duration d

 join mig.ProductAndDuration() pd on d.duration=pd.DurationName