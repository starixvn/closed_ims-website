CREATE PROCEDURE [dbo].[IMS_LicenseeCurrencySel]
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

	BEGIN
	declare @tempLicenseeCurr as table (currency int)
	insert into @tempLicenseeCurr
		SELECT distinct lc.Currency 
		FROM dbo.LicenseeCust lc (nolock)
		WHERE	(@LicenseeID = 0 OR LicenseeID = @LicenseeID)
				AND Currency <> 20
	SELECT 
		t.currency, 
		CASE 
			WHEN t.currency = 51 THEN 'INH'
			ELSE e.Code 
		END AS CurrencyName
	FROM 
		@tempLicenseeCurr t
	INNER JOIN bodb02.dbo.exchange e WITH (NOLOCK) ON t.currency = e.exchangeid
	ORDER BY 
		CASE 
			WHEN t.currency = 51 THEN 'INH'
			ELSE e.Code 
		END
	END
    
GO
GRANT EXECUTE ON [dbo].[IMS_LicenseeCurrencySel] TO [bodbAdminNet]
--GO
--GRANT VIEW DEFINITION ON [dbo].[IMS_LicenseeCurrencySel] TO [vnteam] 