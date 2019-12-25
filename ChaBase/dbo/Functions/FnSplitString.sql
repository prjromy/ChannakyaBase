CREATE FUNCTION [dbo].[FnSplitString] ( @String NVARCHAR(MAX), @Delimiter CHAR(1) ) 
RETURNS @Output TABLE(SplitData NVARCHAR(MAX)) 
BEGIN 
    DECLARE @Start INT, @End INT 
    SELECT @Start = 1, @End = CHARINDEX(@Delimiter, @String) 
    WHILE @Start < LEN(@String) + 1 
	BEGIN 
        IF @End = 0  
            SET @End = LEN(@String) + 1
       
        INSERT INTO @Output (SplitData) VALUES(SUBSTRING(@String, @Start, @End - @Start)) 
        SET @Start = @End + 1 
        SET @End = CHARINDEX(@Delimiter, @String, @Start)        
    END 
    RETURN 
END