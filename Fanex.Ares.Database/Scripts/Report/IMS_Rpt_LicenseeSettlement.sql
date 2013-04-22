CREATE PROCEDURE [dbo].[IMS_Rpt_LicenseeSettlement]
	@licenseeId INT,
	@fromDate DATETIME,
	@toDate DATETIME,
	@product INT,
	@currency INT
AS
	/*  Created: Kevin
		Date: 20130418
		Task : Report - Licensee Settlement Report
		DB: DBARC.bodb_Archive
		Revisions:  
		Param's Explanation:  	
			+ @licenseeId:
			+ @fromDate:
			+ @toDate:
			+ @product:
			+ @currency:
	*/  
	SET NOCOUNT ON

	SELECT 
		GETDATE() AS winlostDate,
		1 AS Currency,
		'SGD' AS CurrencyName,
		0 AS Turnover,
		0 AS BetCount,
		0 AS MemberWinLoss,
		0 AS MemberComm,
		0 AS MemberTotal,
		0 AS LicenseeWinLoss,
		0 AS LicenseeComm,
		0 AS LicenseeTotal,
		0 AS Company,
		0 AS CompanyEUR,
		0 AS CompanyUSD

GO
GRANT EXECUTE ON [dbo].[IMS_Rpt_LicenseeSettlement] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[IMS_Rpt_LicenseeSettlement] TO [vnteam] 