CREATE TABLE [Mast].[TaskDetails] (
    [Task2Id]    INT      IDENTITY (1, 1) NOT NULL,
    [Task1Id]    INT      NOT NULL,
    [TaskTo]     INT      NOT NULL,
    [VerifiedOn] DATETIME NULL,
    [SeenOn]     DATETIME NULL,
    [RejectedOn] DATETIME NULL,
    CONSTRAINT [PK_TaskDetails] PRIMARY KEY CLUSTERED ([Task2Id] ASC),
    CONSTRAINT [FK_TaskDetails_Task] FOREIGN KEY ([Task1Id]) REFERENCES [Mast].[Task] ([Task1Id])
);


GO
create trigger [Mast].[TrigUpdateTaskVerifiedStatus] 
on
 [Mast].[TaskDetails]
 after update
 as 
	   declare @task1id int
	   declare @Verifiedon DateTime
	    declare @Rejectedon DateTime
	   declare @TaskTo int
	   
	     SELECT    
			 @task1id=inserted.task1id,
			 @Verifiedon = inserted.verifiedon,
			@TaskTo=inserted.TaskTo,
			@Rejectedon=inserted.rejectedon
	   from inserted
       SET NOCOUNT ON;
	   begin
	     IF UPDATE(Verifiedon)
			begin
			 if( @Verifiedon is not null)
				begin
					update mast.Task set [IsVerified]=1 where mast.Task.Task1Id=@task1id
				end
			 end
			  IF UPDATE(Rejectedon)
			begin
			 if( @Rejectedon is not null)
				begin
					update mast.Task set [IsVerified]=1 where mast.Task.Task1Id=@task1id
				end
			 end
	   end