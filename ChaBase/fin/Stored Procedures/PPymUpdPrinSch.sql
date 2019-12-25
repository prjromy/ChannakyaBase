Create PROCEDURE [fin].[PPymUpdPrinSch](@IACCNO INT,@AMOUNT MONEY)
---MARKED ENCRYPTION 
AS
DECLARE @schdate datetime
DECLARE @pdprin money
DECLARE @schprin money
DECLARE @mindate datetime

BEGIN

SELECT @mindate=min(schdate) FROM fin.alsch WHERE iaccno = @iaccno AND (ISNULL(schprin,0)-ISNULL(pdprin,0)) <> 0
BEGIN TRANSACTION updt
DECLARE crsr CURSOR LOCAL
		KEYSET
		FOR SELECT ISNULL(schprin, 0),ISNULL(pdprin, 0),schdate FROM fin.ALSch WHERE schdate >= @mindate AND iaccno = @iaccno ORDER BY schDate

		OPEN crsr

		FETCH NEXT FROM crsr INTO @schprin,@pdprin,@schdate
		
		WHILE (@@fetch_status = 0) 
		BEGIN		
			IF @amount > 0 
			BEGIN
					
					IF (@schprin <=(@pdprin + @amount))
					BEGIN
						UPDATE fin.alsch SET pdprin=@schprin WHERE schdate=@schdate AND iaccno = @iaccno
						SET @amount=@amount - (@schprin - @pdprin)
					END
					ELSE
					IF (@schprin > (@pdprin + @amount))
					BEGIN
						UPDATE fin.alsch SET pdprin = @pdprin + @amount WHERE schdate=@schdate AND iaccno = @iaccno
						SET @amount = 0
					END	
			END				
		
		FETCH NEXT FROM crsr INTO @schprin,@pdprin,@schdate
		END
				
		CLOSE crsr
		DEALLOCATE crsr
COMMIT TRANSACTION updt
END