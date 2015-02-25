IF NOT EXISTS(SELECT TOP 1 * FROM sys.tables WHERE name = 'GeoPoints')
BEGIN
	CREATE TABLE GeoPoints(
		Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
		GeoCoordinates Geography
	)

	DECLARE @ammountOfTestRecords AS INT 
	SET @ammountOfTestRecords = 50000
	DECLARE @iterator AS INT
	SET @iterator = 0;

	WHILE @iterator < @ammountOfTestRecords
	BEGIN 
		INSERT INTO GeoPoints (GeoCoordinates) VALUES (geography::Point(RAND(), RAND(), 4326))
		SET @iterator = @iterator + 1
	END

END 

IF EXISTS(SELECT TOP 1 * FROM sys.indexes WHERE name = 'IX_GeoPoints_GeoCoordinates')
BEGIN
  DROP INDEX IX_GeoPoints_GeoCoordinates ON GeoPoints
END

CREATE SPATIAL INDEX IX_GeoPoints_GeoCoordinates
	 ON GeoPoints (GeoCoordinates)
	 USING GEOGRAPHY_AUTO_GRID