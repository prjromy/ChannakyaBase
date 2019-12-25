
CREATE FUNCTION [dbo].[FGetMatDat] (  
  
 @DateAD DATE  
  
 ,@AddMonthDay NUMERIC(11, 3)  
  
 ,@IsDateBS BIT  
  
 )  
  
RETURNS DATE  
  
AS  
  
BEGIN  
  
 ----  
  
 DECLARE @AddMonth INT  
  
 DECLARE @AddDay INT  
  
 DECLARE @AddYear INT  
  
  
  
 --declare @AddMonthDay numeric(5,2) = -3.02  
  
 --select value from dbo._FNSplit(@AddMonthDay,'.') x where idx = 0   
  
 --select value*-1 from dbo._FNSplit(@AddMonthDay,'.') x where idx = 1   
  
 SET @AddMonth = isnull((  
  
    SELECT value  
  
    FROM dbo._FNSplit(@AddMonthDay, '.') x  
  
    WHERE idx = 0  
  
    ), 0)  
  
  
  
 IF @AddMonthDay < 0  
  
 BEGIN  
  
  SET @AddDay = isnull((  
  
     SELECT value * - 1  
  
     FROM dbo._FNSplit(@AddMonthDay, '.') x  
  
     WHERE idx = 1  
  
     ), 0)  
  
 END  
  
 ELSE  
  
 BEGIN  
  
  SET @AddDay = isnull((  
  
     SELECT value  
  
     FROM dbo._FNSplit(@AddMonthDay, '.') x  
  
     WHERE idx = 1  
  
     ), 0)  
  
 END  
  
  
  
 IF @IsDateBS = 0  
  
 BEGIN  
  
  SET @DateAD = dateadd(month, @AddMonth, @DateAD)  
  
  SET @DateAD = DATEADD(day, @AddDay, @DateAD)  
  
  ----if @AddMonth <> 0  
  
  SET @DateAD = DATEADD(day, - 1, @DateAD)  
  
 END  
  
 ELSE  
  
 BEGIN  
  
  DECLARE @DateBS VARCHAR(10)  
  
  DECLARE @BSYear SMALLINT  
  
   ,@BSMonth SMALLINT  
  
   ,@BSDay SMALLINT  
  
  DECLARE @NewYear INT  
  
   ,@NewMonth INT  
  
   ,@NewDay INT  
  
  
  
  SET @DateBS = dbo.FGetDateBS(@DateAD)  
  
  SET @BSYear = dbo.FGetBSYear(@DateBS)  
  
  SET @BSMonth = dbo.FGetBSMonth(@DateBS)  
  
  SET @BSDay = dbo.FgetBSDay(@DateBS)  
  
  SET @AddYear = floor((@BSMonth + @AddMonth) / 12)  
  
  SET @NewMonth = (@AddMonth + @BSMonth) % 12  
  
  
  
  IF @NewMonth = 0  
  
  BEGIN  
  
   --  set @AddYear  = @AddYear - 1  
  
   SET @NewYear = @BSYear + @AddYear - 1  
  
   SET @NewMonth = 12  
  
  END  
  
  ELSE  
  
  BEGIN  
  
   SET @NewYear = @BSYear + @AddYear  
  
  END  
  
  
  
  IF dbo.FGetDaysInMonth(@NewYear, @NewMonth) < @BSDay  
  
  BEGIN  
  
   SET @BSDay = dbo.FGetDaysInMonth(@NewYear, @NewMonth)  
  
  END  
  
  
  
  SET @DateAd = dbo.FGetDateAD(@NewYear, @NewMonth, @BSDay)  
  
  SET @DateAD = dateadd(day, @AddDay, @DateAD)  
  
  SET @DateAD = DATEADD(day, - 1, @DateAD)  
  
 END  
  
  
  
 RETURN @DateAd  
  
END