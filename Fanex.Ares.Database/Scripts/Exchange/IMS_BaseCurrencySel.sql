CREATE PROCEDURE [dbo].[IMS_BaseCurrencySel]
AS
	/*  Created: Kevin
		Date: 20130418
		Task : Report - Licensee Settlement Report
		DB: DB052.bodb02
		Revisions:  
		Param's Explanation: 
	*/  
	SET NOCOUNT ON

		DECLARE	@tmpCurrency TABLE(currency INT PRIMARY KEY, currencyname NVARCHAR(50))

	INSERT INTO @tmpCurrency
		SELECT e.exchangeid, e.Code FROM dbo.Exchange e	WITH(NOLOCK)
		WHERE exchangeid IN(3, 6)
  
	SELECT * FROM @tmpCurrency    

GO
GRANT EXECUTE ON [dbo].[IMS_BaseCurrencySel] TO [bodbAdminNet]
--GO
--GRANT VIEW DEFINITION ON [dbo].[IMS_ExchangeRateBillingSel] TO [vnteam] 