CREATE PROCEDURE [dbo].[IMS_LicenseeBillingInfoSel]
	@licenseeId INT = 0,
	@productId INT = 0
AS
	/*  Created: Kevin
		Date: 20130418
		Task : Report - Licensee Settlement Report
		DB: 
		Revisions:  
		Param's Explanation:  
		  + @productId: 1 - sportbook, 2 - ;  
	*/  
	SET NOCOUNT ON

	SELECT 
		GETDATE() AS FirstBilling, 
		'15% of Net Win' AS BasisOfBilling, 
		'EUR15000 (minimum fees waived 3 months from date of launch)' AS MinimumFees,  
		GETDATE() AS HeadOfTerms

GO
GRANT EXECUTE ON [dbo].[IMS_LicenseeBillingInfoSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[IMS_LicenseeBillingInfoSel] TO [vnteam] 