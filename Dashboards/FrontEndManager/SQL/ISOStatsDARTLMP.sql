select Market,'RT LMP' as DataPoint,  [RT LMP] AS VALUE, Timepoint, Location from (  
SELECT  name.LocationName as Location ,pubs.PublisherName  as  Market, price1.StatDateTime as Timepoint , price1.StatValue AS LMP, type1.StatType AS STYPE
FROM 
((((
ISOStats.dbo.StatDetails as price1 
INNER JOIN ISOStats.dbo.StatSummary  as map1 ON price1.StatID=map1.StatID and price1.StatDateTime >= '2015-10-01' and price1.StatDateTime < '2015-10-02' )
INNER JOIN ISOStats.dbo.LOcations as name   ON name.LocationID=map1.LocationID)
INNER JOIN ISOStats.dbo.StatTypes as type1 ON map1.StatTypeID=type1.StatTypeID )  
INNER JOIN [ISOStats].[dbo].[Publishers] as pubs ON name.PublisherID = pubs.PublisherID)
WHERE type1.StatType in ('RT LMP') and name.LocationName in('WR.MW.GMEC.MW', 'CWLP.DALLMA91','5021504') )as SourceTable
pivot 
(MAX(LMP)  FOR STYPE in ([RT LMP])) AS PivotTable ;  