CREATE FUNCTION [fin].[FCHECKISGUARANTORORLOANEE] (@IACCNO INT)





RETURNS @CHKGUARANTOR TABLE (AccountNumber VARCHAR(25),BlockedAmt MONEY,DisplayMsg BIT,IsLoanee BIT,IsPercent bit)

---MARKED ENCRYPTION 

as

Begin

        -- Declare @blockedAmt Money

         --Declare @LoanAcNo VarChar(25)

         --Declare @DispMsg Bit

        -- Declare @isLoanee Bit

		  --Declare @IsPercent Bit

	--Declare Crsr Cursor For 

	

	Insert Into @CHKGUARANTOR (AccountNumber,BlockedAmt,DisplayMsg,IsLoanee,IsPercent)

	

	SELECT     adetail.accno as AccountNumber, IsNull(fin.Guarantor.BlockedAmt,0) as BlockedAmt , DisplayMsg, 0 as IsLoanee,IsPercent

	FROM         fin.Guarantor INNER JOIN

		                    fin.ADetail ON fin.Guarantor.LIaccno = fin.ADetail.IAccno

	WHERE     (fin.Guarantor.GIaccno = @IACCNO) and adetail.accstate = 1 and Status=1

	union all

	

	SELECT  fin.ADetail.Accno as AccountNumber ,IsNull(fin.ADetail.Bal,0) as BlockedAmt, '' DisplayMsg , 1 as  IsLoanee, 0 as IsPercent

			FROM         fin.ADetail INNER JOIN

		                      fin.ALinkloan ON fin.ADetail.IAccno = fin.ALinkloan.ILinkAc

			WHERE     (fin.ALinkloan.ILinkAc = @IACCNO) AND (fin.ADetail.AccState = 1)











	--Open Crsr

	--Fetch From Crsr Into @blockedAmt,@loanAcno,@DispMsg,@isloanee, @IsPercent

	--While @@Fetch_Status = 0

	--Begin

		--If IsNull(@loanAcno,'') = '' 

	    	--Begin

			--SELECT    @loanAcno= fin.ADetail.Accno, @blockedAmt=fin.ADetail.Bal,@isloanee = 1

			--FROM         fin.ADetail INNER JOIN

		                      --fin.ALinkloan ON fin.ADetail.IAccno = fin.ALinkloan.IAccno

			--WHERE     (fin.ALinkloan.ILinkAc = @iaccno) AND (fin.ADetail.AccState = 1)

	

		--End

	

	--If IsNull(@loanAcno,'') <> '' 

		

	--Fetch Next From Crsr Into @blockedAmt,@loanAcno,@DispMsg,@isloanee,@IsPercent

	--End

	--Close Crsr

	--Deallocate Crsr

Return

End