-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FGetWeekHolliday] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(

SELECT 
     Split.a.value('.', 'VARCHAR(100)') AS WeekHoliday  
FROM  
(
    SELECT CAST ('<M>' + REPLACE((SELECT PValue
  FROM [LG].[ParamValue] where pid=3006), ',', '</M><M>') + '</M>' AS XML) AS CVS 
) AS A CROSS APPLY CVS.nodes ('/M') AS Split(a)
)