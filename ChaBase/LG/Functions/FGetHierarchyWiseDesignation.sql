Create function [LG].[FGetHierarchyWiseDesignation]()
returns table
as return
(select pv.PId,pv.PValue,PName,DegOrder=
 case 
	when pv.Pid=6007 then 1 
	when pv.Pid=6008 then 2 
    when pv.Pid=2007 then 3 
    when pv.Pid=2008 then 4    
    when pv.Pid=2005 then 5 
	when pv.Pid=2006 then 6 
	when pv.Pid=2009 then 7
	when pv.Pid=6009 then 8
end
   from lg.ParamValue pv inner join lg.Param pm on pv.PId=pm.PId  where ParentId=2004 )