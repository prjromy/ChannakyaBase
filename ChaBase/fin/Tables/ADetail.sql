CREATE TABLE [fin].[ADetail] (
    [IAccno]        INT             IDENTITY (1, 1) NOT NULL,
    [PID]           INT             NOT NULL,
    [Accno]         VARCHAR (50)    NULL,
    [RDate]         SMALLDATETIME   NOT NULL,
    [CurrID]        INT             NOT NULL,
    [BrchID]        INT             NULL,
    [PostedBy]      VARCHAR (30)    NOT NULL,
    [ApprovedBy]    VARCHAR (30)    NULL,
    [AccState]      TINYINT         NOT NULL,
    [Aname]         VARCHAR (100)   NOT NULL,
    [OldAccno]      VARCHAR (50)    NULL,
    [Bal]           MONEY           CONSTRAINT [DF_ADetail_Bal] DEFAULT ((0)) NOT NULL,
    [IonBal]        MONEY           CONSTRAINT [DF_ADetail_IonBal] DEFAULT ((0)) NOT NULL,
    [IRate]         DECIMAL (18, 2) CONSTRAINT [DF_ADetail_IRate] DEFAULT ((0)) NOT NULL,
    [DateType]      TINYINT         CONSTRAINT [DF_ADetail_DateType] DEFAULT ((1)) NOT NULL,
    [LastTransDate] DATE            NULL,
    CONSTRAINT [PK_AccountDetail] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_ADetail_AccountStatus] FOREIGN KEY ([AccState]) REFERENCES [fin].[AccountStatus] ([AsId]),
    CONSTRAINT [FK_ADetail_CurrencyType] FOREIGN KEY ([CurrID]) REFERENCES [fin].[CurrencyType] ([CTId]),
    CONSTRAINT [FK_ADetail_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID])
);


GO
CREATE NONCLUSTERED INDEX [IX_ADetail]
    ON [fin].[ADetail]([BrchID] ASC, [Accno] ASC, [AccState] ASC, [Aname] ASC, [PID] ASC, [RDate] ASC);


GO
CREATE TRIGGER [fin].[TrgAccountNumberGenerate]
       ON fin.ADetail
AFTER insert,update
AS
	   declare @Product varchar(10)
	   declare @Currency varchar(5)
	   declare @Branch varchar(5)
	   declare @ProductID int
	   declare @accountnumber varchar(50)
	   declare @accountnumberSequence varchar(3)
	   declare @StartingAcNo varchar(30)
	   declare @LastAcNo int
	   declare @IAccno bigint
	   declare @PrevAccstate int
	   declare @ChangedAccstate int
	   declare @AppBy int
       SET NOCOUNT ON;
	   
       SELECT    
			 @ProductID=inserted.PID,
			 @Branch = REPLICATE('0',3-LEN(RTRIM(inserted.BrchID))) + RTRIM(inserted.BrchID),
			 @Currency= REPLICATE('0',3-LEN(RTRIM(inserted.CurrID))) + RTRIM(inserted.CurrID),
			 @IAccno=inserted.IAccno,
			 @ChangedAccstate=inserted.AccState,
			 @AppBy =inserted.ApprovedBy
	   from inserted
	   select @PrevAccstate= deleted.AccState 
	   from deleted
	   set @LastAcNo=(select top 1 lastacno  from fin.productbrnch where pid=@ProductID and brnchid=@Branch)
	   set @StartingAcNo= REPLICATE('0',8-LEN(RTRIM(@LastAcNo))) + RTRIM(@LastAcNo)
	   select @accountnumberSequence=  pvalue from lg.ParamValue where pid=1001
	   select @Product= ProductDetail.Apfix from fin.ProductDetail where pid=@ProductID
	  if(@Branch=0)
		 begin
			 set @Branch=1
		 end
	   set @Product=ISNULL(@Product,@ProductID)
	   set @Branch=isnull(@Branch,0)
	   set @Currency=isnull(@currency,0)
	   set @StartingAcno=isnull(@StartingAcno,0)
BEGIN
	IF EXISTS(SELECT * FROM INSERTED)  AND EXISTS(SELECT * FROM DELETED) 
        BEGIN 
		   IF UPDATE(AccState) 
			begin
				if(@PrevAccstate=6 and @ChangedAccstate=1 and @AppBy is not null)
					begin
						if(@accountnumberSequence=123)
								set @accountnumber=@Branch+'-'+@Currency+'-'+@Product
						else if(@accountnumberSequence=132)
								set @accountnumber=@Branch+'-'+@Product+'-'+@Currency
						else if(@accountnumberSequence=213)
								set @accountnumber=@Currency+'-'+@Branch+'-'+@Product
						else if(@accountnumberSequence=231)
								set @accountnumber=@Currency+'-'+@Product+'-'+@Branch
						else if(@accountnumberSequence=321)
								set @accountnumber=@Product+'-'+@Currency+'-'+@Branch
						else if(@accountnumberSequence=312)
								set @accountnumber=@Product+'-'+@Branch+'-'+@Currency

						set @accountnumber=@accountnumber+'-'+@StartingAcNo
						update fin.ADetail set Accno=@accountnumber where IAccno=@IAccno
						update [fin].[ProductBrnch] set lastacno=@LastAcNo+1 where  pid=@ProductID and brnchid=@Branch
					end
		    END 
		end

 ELSE IF EXISTS(SELECT * FROM INSERTED)  AND NOT EXISTS(SELECT * FROM DELETED) 
        BEGIN  
			update fin.ADetail set Accno=concat('T-',(REPLICATE('0',16-LEN(RTRIM(@IAccno+09111993))) + RTRIM(@IAccno+09111993))) where IAccno=@IAccno
		End
end