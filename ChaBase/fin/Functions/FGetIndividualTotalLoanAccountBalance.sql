CREATE FUNCTION [fin].[FGetIndividualTotalLoanAccountBalance]

(		

	@TDATE SMALLDATETIME

)

RETURNS TABLE

---MARKED ENCRYPTION 

As

Return	

	Select 

		Z.IAccNo,

		Sum(PrinOut) 'PrinOut',

		Sum(Interest - InterestP) 'IntOut',

		Sum(POnPr - POnPrP) 'POnPrOut',

		Sum(POnI - POnIP) 'POnIOut',

		Sum(IOnI - IOnIP) 'IOnIOut' 

	From	(Select 

			IAccNo,

			Sum(PrinOut)  'PrinOut',

			Sum(Int) 'Interest',

			Sum(PonPr) 'POnPr',

			Sum(PonI) 'POnI',

			Sum(IonI) 'IOnI',	

			Sum(InterestP)'InterestP',

			Sum(POnPrP)'POnprP',

			Sum(POnIP)'POnIP',

			Sum(IOnIP)'IOnIP' 

		From 	(Select 

				IAccNo,

				0 'PrinOut',

				'Int' = Sum(Case When Valued = 0  Then IsNull(IntCal,0) Else 0 End + Case When Valued = 5  Then  IsNull(IntCal,0) Else 0 End),			

				'PonPr' = Sum(Case When Valued = 3 Then  IsNull(IntCal,0) Else 0 End + Case When Valued = 8 Then   IsNull(IntCal,0) Else 0 End),			

				'PonI' = Sum(Case When Valued = 4 Then  IsNull(IntCal,0) Else 0 End + Case When Valued = 9 Then   IsNull(IntCal,0) Else 0 End),

				'IonI' = Sum(Case When Valued = 1 Then  IsNull(IntCal,0) Else 0 End + Case When Valued = 6 Then  IsNull(IntCal,0) Else 0 End) ,

				0 'InterestP',

				0 'POnPrP',

				0 'POnIP',

				0 'IOnIP'

			From  fin.IntLog 

			Where Iaccno In (Select IAccNo From fin.FGetAccType(1)) And TDate <= @TDate  Group By IAccNo  

			Union All

			Select 

				IAccNo,

				0 'PrinOut',

				'Int' = Sum(Case When PID = 0  Then IsNull(Amt,0) Else 0 End + Case When PID = 5  Then IsNull(Amt,0) Else 0 End),			

				'PonPr' = Sum(Case When PID = 3 Then  IsNull(Amt,0) Else 0 End + Case When PID = 8 Then  IsNull(Amt,0) Else 0 End),			

				'PonI' = Sum(Case When PID = 4 Then  IsNull(Amt,0) Else 0 End + Case When PID = 9 Then  IsNull(Amt,0) Else 0 End),

				'IonI' = Sum(Case When PID = 1 Then  IsNull(Amt,0) Else 0 End + Case When PID = 6 Then  IsNull(Amt,0) Else 0 End),

				0 'InterestP',

				0 'POnPrP',

				0 'POnIP',

				0 'IOnIP'

			From 

				fin.Aadjustmnt 

			Where 

				IAccNo In (Select IAccNo From fin.FGetAccType(1)) And TDate <= @TDate  Group By IAccNo  

			Union All

			Select 

				IAccNo,

				'PrinOut' = Sum(Case When PID = 10  Then IsNull(Amt,0) Else 0 End - Case When PID = 13  Then IsNull(Amt,0) Else 0 End),

				0 'Int',

				0 'PonPr',

				0 'PonI',

				0 'IonI' ,

				'InterestP' = Sum(Case When PID = 0  Then IsNull(Amt,0) Else 0 End + Case When PID = 5  Then IsNull(Amt,0) Else 0 End),

				'POnPrP' = Sum(Case When PID = 3 Then  IsNull(Amt,0) Else 0 End + Case When PID = 8 Then  IsNull(Amt,0) Else 0 End),

				'POnIP' = Sum(Case When PID = 4 Then  IsNull(Amt,0) Else 0 End + Case When PID = 9 Then  IsNull(Amt,0) Else 0 End),

				'IOnIP' = Sum(Case When PID = 1 Then  IsNull(Amt,0) Else 0 End + Case When PID = 6 Then  IsNull(Amt,0) Else 0 End)

			From 

				trans.AmTrns 

			Inner Join 

				fin.AmTransLoan 

			On 

				AmTrns.TNo = AmTransLoan.TNo 

			Where 

				VDate <= @TDate

			Group By IAccNo

			)X Group By IAccNo

		) Z inner join fin.adetail ad on ad.iaccno=z.iaccno

		Group By Z.IAccNo