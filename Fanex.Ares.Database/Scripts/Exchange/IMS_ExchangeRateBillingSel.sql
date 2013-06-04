CREATE PROCEDURE [dbo].[IMS_ExchangeRateBillingSel]
	@currency VARCHAR(500) = '',
	@exchangeDate DATETIME,
	@baseCurrencyId INT	= 6
AS
	/*  Created: Kevin
		Date: 20130418
		Task : Report - Licensee Settlement Report
		DB: DBLIC.bodb_Licensee
		Revisions:  
		Param's Explanation:  	
			+ @currency: currency id list
			+ @exchangeDate:
			+ @baseCurrencyId 
	*/  
	SET NOCOUNT ON

	DECLARE	@tmpCurrency TABLE(currency INT PRIMARY KEY)

	INSERT INTO @tmpCurrency
	SELECT DISTINCT val
	FROM strSPlit(@currency,',')

	SELECT * INTO #TempExchange 
	FROM bodb02.dbo.Lrf_f_GetExchangeDaily(@exchangeDate, @exchangeDate)
	WHERE exchangeid IN (SELECT currency from @tmpCurrency)

	SELECT 
		e.exchangeid AS Currency, 
		e.Code AS  CurrencyName, 
		(CASE WHEN(@baseCurrencyId = 6)
		THEN
			e.EUR
		ELSE
			e.USD
		END) AS BasePerUnit,
		1/
		(CASE WHEN(@baseCurrencyId = 6)
		THEN
			e.EUR
		ELSE
			e.USD
		END) AS UnitsPerBase	 
	FROM #TempExchange e	 

	DROP TABLE #TempExchange

GO
GRANT EXECUTE ON [dbo].[IMS_ExchangeRateBillingSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[IMS_ExchangeRateBillingSel] TO [vnteam] 