CREATE PROCEDURE [fin].[PPymUpdIntSch](@IACCNO INT,@AMOUNT MONEY,@PDATE DATETIME)
---MARKED ENCRYPTION 
AS
DECLARE @schdate datetime
DECLARE @pdint money
DECLARE @calcint money
DECLARE @mindate datetime

BEGIN

SELECT @mindate=min(schdate) FROM alsch WHERE iaccno = @iaccno AND (ISNULL(calcint,0)-ISNULL(pdint,0)) <> 0
BEGIN TRANSACTION updt
DECLARE crsr CURSOR LOCAL
		KEYSET
		FOR SELECT ISNULL(calcInt, 0),ISNULL(pdInt, 0),schdate FROM fin.ALSch WHERE schdate >= @mindate AND iaccno = @iaccno ORDER BY schDate

		OPEN crsr

		FETCH NEXT FROM crsr INTO @calcint,@pdint,@schdate
		
		WHILE (@@fetch_status = 0) 
		BEGIN		
			IF @amount > 0 
			BEGIN
				IF @schdate <= @pdate
				BEGIN			
					IF (@calcint <=(@pdint + @amount))
					BEGIN
						UPDATE fin.alsch SET pdint=@calcint WHERE schdate=@schdate AND iaccno = @iaccno
						SET @amount=@amount-(@calcint-@pdint)
					END
					ELSE
					IF (@calcint > (@pdint + @amount))
					BEGIN
						UPDATE fin.ALSch SET pdint = @pdint + @amount WHERE schdate=@schdate AND iaccno = @iaccno
						SET @amount = 0
					END								
				END
				ELSE
				IF @schdate > @pdate
				BEGIN
					UPDATE fin.alsch SET pdint = @pdint + @amount WHERE schdate=@schdate AND iaccno = @iaccno
					SET @amount=0
				END
			END				
		
		FETCH NEXT FROM crsr INTO @calcint,@pdint,@schdate
		END
				
		CLOSE crsr
		DEALLOCATE crsr
COMMIT TRANSACTION updt
END