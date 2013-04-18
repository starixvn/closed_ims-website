CREATE PROCEDURE [dbo].[IMS_ExchangeRateBillingSel]
	@currency VARCHAR(500) = '',
	@exchangeDate DATETIME
AS
	/*  Created: Kevin
		Date: 20130418
		Task : Report - Licensee Settlement Report
		DB: 
		Revisions:  
		Param's Explanation:  	
			+ @currency: currency id list
			+ @exchangeDate: 
	*/  
	SET NOCOUNT ON

	DECLARE	@tmpCurrency TABLE(currency INT PRIMARY KEY)

	INSERT INTO @tmpCurrency
	SELECT val
	FROM strSPlit(@currency,',')

	SELECT * INTO #TempExchange 
	FROM Lrf_f_GetExchangeDaily(@exchangeDate, @exchangeDate)
	WHERE exchangeid IN (SELECT currency from @tmpCurrency)

	SELECT 
		e.exchangeid AS currency, 
		e.Code AS  currencyName, 
		e.EUR AS EURPerUnit,
		(1/e.EUR) AS unitsPerEUR,
		e.USD AS USDPerUnit,
		(1/e.USD) AS unitsPerUSD
	FROM #TempExchange e	 

	DROP TABLE #TempExchange

GO
GRANT EXECUTE ON [dbo].[IMS_ExchangeRateBillingSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[IMS_ExchangeRateBillingSel] TO [vnteam] 