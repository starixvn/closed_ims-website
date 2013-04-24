CREATE PROCEDURE [dbo].[IMS_LicenseeProductSel]
	@licenseeId INT = 0
AS
	/*  Created: Kevin
		Date: 20130423
		Task : Report - Licensee Settlement Report
		DB: DBLIC.bodb_Licensee
		Revisions:  
		Param's Explanation:  
		
	*/  
	SET NOCOUNT ON

	DECLARE @products AS TABLE (productid INT PRIMARY KEY, productname VARCHAR(50))

	INSERT INTO @products(productid, productname)  VALUES(0, 'Sportsbook')
	INSERT INTO @products(productid, productname)  
		SELECT ld.Productid, p.ProductName FROM dbo.LicenseeProduct3rd ld WITH (NOLOCK)
		LEFT JOIN dbo.Product3rd p
		ON  ld.ProductID = p.Productid
		WHERE ld.LicenseeID = @licenseeId
	SELECT * FROM @products p ORDER BY p.productid

GO
GRANT EXECUTE ON [dbo].[IMS_LicenseeProductSel] TO [bodbAdminNet]
--GO
--GRANT VIEW DEFINITION ON [dbo].[IMS_LicenseeProductSel] TO [vnteam] 