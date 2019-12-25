CREATE Function [LG].[FGetConnectionIDFromTask](@TaskID int)
returns table as return(
select TD.TaskTo as UserId,Isnull(UC.ConnectionID,'') as ConnectionID, (select count(Task1Id) as Countl from mast.FGetTaskDetails(TD.TaskTo) where SeenOn is null and IsVerified=0
) as CountNotification from Mast.TaskDetails TD 
left join LG.UserConnection UC on TD.TaskTo=UC.UserID
where TD.Task1Id=@TaskID
)