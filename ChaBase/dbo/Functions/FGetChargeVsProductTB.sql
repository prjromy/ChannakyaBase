CREATE function [dbo].[FGetChargeVsProductTB]()
returns table
as return(
SELECT ChgId, LTRIM(RTRIM(m.n.value('.[1]','nvarchar(max)'))) AS product
FROM ( SELECT ChgId,CAST('<XMLRoot><RowData>' + REPLACE(Product,',','</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
       FROM fin.ChargeDetail  )t
CROSS APPLY x.nodes('/XMLRoot/RowData')m(n))