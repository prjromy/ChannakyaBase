CREATE function mig.ProductCalandCapRule()



returns table as return







Select * from (Select SDName,PName,IAccNo,AccNo,AName,SType,



'MinBal' = Case When Exists (Select * From nimbus.dbo.AMinBal Where IAccNo = ADetail.IAccNo) Then

			(Select MinBal From nimbus.dbo.AMinBal Where IAccNo = ADetail.IAccNo) Else

			ProductDetail.LAmt End,



'PSID' =  Case 


			When Exists

                	(Select * From nimbus.dbo.APolInt Where APolInt.IAccNo = ADetail.IAccNo) Then



                       	(Select nimbus.dbo.PolicyIntCalc.PSID From nimbus.dbo.APolInt 


			Inner Join nimbus.dbo.PolicyIntCalc On nimbus.dbo.APolInt.PSID = nimbus.dbo.PolicyIntCalc.PSID


                        Where  (APolInt.IAccNo = ADetail.IAccNo)) 

			When Exists



                        (SELECT * From nimbus.dbo.ProductPSID Where Pid = ADetail.pid) Then



                        (SELECT nimbus.dbo.PolicyIntCalc.PSID From nimbus.dbo.ProductPSID 



			Inner Join nimbus.dbo.PolicyIntCalc ON nimbus.dbo.ProductPSID.PSID = nimbus.dbo.PolicyIntCalc.PSID 



                        Where (nimbus.dbo.ProductPSID.IsDefault = 1) And (nimbus.dbo.ProductPSID.PID = ADetail.PID) )End,



	'ICBDurID' = Case When Exists



                       	(Select * From nimbus.dbo.AICBDur Where IAccNo = Adetail.IAccNo) Then



                          	(Select RuleICBDuration.ICBDurID From nimbus.dbo.RuleICBDuration 



				Inner Join nimbus.dbo.AICBDur ON RuleICBDuration.ICBDurID = AICBDur.ICBDurID



                        	Where IAccNo = ADETAIL.IAccNo) Else 



			(Select RuleICBDuration.ICBDurID  From nimbus.dbo.ProductICBDur 



			Inner Join nimbus.dbo.RuleICBDuration On nimbus.dbo.ProductICBDur.ICBDurID = nimbus.dbo.RuleICBDuration.ICBDurID 



                        Where (nimbus.dbo.ProductICBDur.IsDefault = 1) AND (nimbus.dbo.ProductICBDur.PID = Adetail.pid)) End



From nimbus.dbo.ADetail



Inner Join nimbus.dbo.ProductDetail On Adetail.PID = ProductDetail.PID



Inner Join nimbus.dbo.SchmDetails On ProductDetail.SDID = SchmDetails.SDID



)X