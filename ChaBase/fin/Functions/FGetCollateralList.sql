-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION  [fin].[FGetCollateralList]
(	

)
RETURNS TABLE 
AS
RETURN 
(
	select ac.Sno,CValue,OName,OAdd,contactNo,RegNo,Remarks,
ISNULL(al.AcolLandId,0) as LandId,
ISNULL(av.ColVechicleId,0) as VehicleId,
ISNULL(ad.AlFixedId,0)as fixedDepoId,
ac.NCID,
nad.Alias as CollateralName,
adet.Aname as AccountName,
adet.IAccno as IAccno,
adet.Accno as AccountNumber
 from fin.ALColl ac
 join fin.ADetail adet on adet.IAccno=ac.IAccno
left join fin.ALCollLand al on ac.Sno=al.Sno
left  join fin.ALCollVehicle av on av.Sno=ac.Sno
left join fin.ALFixedDeposit ad on ad.Sno=ac.Sno
left join fin.NCollateralDetail nad on nad.NCId=ac.NCID
)