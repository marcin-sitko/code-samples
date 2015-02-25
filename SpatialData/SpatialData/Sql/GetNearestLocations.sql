DECLARE @g GEOGRAPHY = geography::Point(RAND(), RAND(), 4326)

SELECT TOP 5 GeoCoordinates.ToString() FROM GeoPoints 
WHERE GeoCoordinates.STDistance(@g) IS NOT NULL
ORDER BY GeoCoordinates.STDistance(@g)