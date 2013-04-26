CREATE PROCEDURE [dbo].[IMS_ExchangeRateBillingSel]
	@currency VARCHAR(500) = '',
	@exchangeDate DATETIME
AS
	/*  Created: Kevin
		Date: 20130418
		Task : Report - Licensee Settlement Report
		DB: DB052.bodb02
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
	FROM IMS_f_ExchangeDailySel(@exchangeDate, @exchangeDate)
	WHERE exchangeid IN (SELECT currency from @tmpCurrency)

	SELECT *
	FROM #TempExchange e	 

	DROP TABLE #TempExchange

GO
GRANT EXECUTE ON [dbo].[IMS_ExchangeRateBillingSel] TO [bodbAdminNet]
--GO
--GRANT VIEW DEFINITION ON [dbo].[IMS_ExchangeRateBillingSel] TO [vnteam] 