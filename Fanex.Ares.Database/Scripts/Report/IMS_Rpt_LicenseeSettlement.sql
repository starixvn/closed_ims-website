CREATE PROCEDURE [dbo].[IMS_Rpt_LicenseeSettlement]
	@licenseeId INT,
	@licenseeName VARCHAR(50),
	@fromDate SMALLDATETIME,
	@toDate SMALLDATETIME,
	@product INT = 0,
	@currency INT = 0,
	@baseCurrency INT = 0,
	@moveBetsDay SMALLDATETIME
AS
	/*  Created: Kevin
		Date: 20130418
		Task : Report - Licensee Settlement Report
		DB: DB052.bodb02
		Revisions:  
		Param's Explanation:  	
			+ @licenseeId:
			+ @fromDate:
			+ @toDate:
			+ @product: 0 -> sportbook, 
			+ @currency:
	*/  
	SET NOCOUNT ON
	
	DECLARE  @isGetOldData BIT
	IF 	@fromDate < '2009-1-1'
		SET @isGetOldData = 1;
	ELSE 
		SET @isGetOldData = 0;
	
	DECLARE @tempAg	TABLE( custID INT,currency INT) 
	DECLARE @VNDCurrencyName VARCHAR(20)			
	DECLARE @bettype AS TABLE (bettype INT PRIMARY KEY)
	DECLARE @sporttype TABLE(sporttype INT)						

	CREATE TABLE #SAgWinLoss  (  	
		winlostdate		DATETIME,
		Currency		INT,
		TurnOver		MONEY,
		Winlost			MONEY,
		PlayerComm 		MONEY,
		LicenseeWL		MONEY,
		LicenseeComm 	MONEY,
		Company			MONEY,
		Betcount		BIGINT
	)
	
	INSERT INTO @tempAg (custID, currency) 
		SELECT custID, currency FROM bodb_Licensee.dbo.LicenseeCust (NOLOCK) 
		WHERE 																
			LicenseeID = @licenseeId										
			AND (@Currency = 0 OR (@Currency<> 0 AND Currency=@Currency))	
			AND (Currency <> 20)
	SELECT @VNDCurrencyName = currency FROM exchange (NOLOCK) WHERE ExchangeId = 51

	DECLARE @Super AS TABLE (custid int primary key, currency int)				   
	INSERT INTO @Super(custid, currency)  
	SELECT CustID, currency FROM Dep_CustSuper WITH (NOLOCK)  
	WHERE  site = @licenseeName			  	

	IF @product = 0  --sportbook										 
	BEGIN  	   
		INSERT  INTO @sporttype( sporttype)
		VALUES  (0)						   
	END  

	DECLARE @TempFromDate DATETIME
	DECLARE @TempToDate DATETIME 	   
	DECLARE @MaxOldDate DATETIME	   
	DECLARE @FDate DATETIME 		   
	DECLARE @TDate DATETIME 		   
	--to Deal With Parameter Sniffing 

	SELECT @FDate=@fromDate, @TDate=@toDate 	
	IF @TDate > @moveBetsDay  
	BEGIN				
		IF @FDate <= @moveBetsDay 
			SET @TempFromDate =  DATEADD(DAY,1,@moveBetsDay)	 
		ELSE												 
			SET @TempFromDate = @FDate

		IF @product = 0  --sportbook			
		BEGIN		
			INSERT INTO #SAgWinLoss
			SELECT   			   
				b.winlostdate,     
				s.currency,		   
				SUM(stake) turnover, 
				SUM(b.winlost - b.actual_stake) AS Winlost,
				SUM(PlayerComm) as PlayerComm,
				SUM(b.awinlost - b.actual_stake) AS LicenseeWL,				
				SUM(b.comm) AS LicenseeComm,
				- SUM(b.awinlost + b.winlost - b.actual_stake) AS Company, 
				COUNT(b.transid) AS BetCount	  
			FROM 			
				Bettrans b WITH (NOLOCK,INDEX=IX_srecommend)
				INNER JOIN @Super s ON b.srecommend = s.custid	  
				INNER JOIN @TempAg a ON b.recommend = a.custid	
			WHERE 											  
				b.winlostdate BETWEEN @TempFromDate AND @TDate AND 	
				b.statusID  IN (111, 112, 113) AND				   
				(b.bettype < 31 OR (b.bettype BETWEEN 121 AND 999)) AND 
				b.tpositiontaking <> 0	 
			GROUP BY   						  
				b.winlostdate,s.currency,statusID	 
		END		  
	END  
	
	IF @FDate <= @moveBetsDay  
	BEGIN 
		IF @TDate >= @moveBetsDay	  
			SET @TempToDate = @moveBetsDay	
		ELSE							   
			SET  @TempToDate = @TDate	  
		
		--exclude BA tickets
		IF @product = 0  --sportbook	 
		BEGIN				
			INSERT INTO #SAgWinLoss   
			SELECT 					
				b.winlostdate,  	
				b.currency, 		 
				SUM(b.turnover),     		   
				SUM(b.winlost),    
				SUM(b.playercomm ) as PlayerComm,
				SUM(CASE WHEN b.sporttype = 0  THEN b.awinlost ELSE 0 END) AS LicenseeWL,				
				SUM(b.comm) as LicenseeComm,
				- SUM(CASE WHEN b.sporttype = 0 THEN b.awinlost + b.winlost ELSE 0 END)  as Company,	  
				SUM(b.betcount)	
			FROM 				
				bodb_Licensee.dbo.rptLicWinloss2 b WITH ( NOLOCK )    
				INNER JOIN @sporttype t ON b.sporttype = t.sporttype 
				INNER JOIN @TempAg ag ON ag.custid = b.recommend	 
			WHERE  								  
				b.Winlostdate BETWEEN @FDate AND @TempToDate  	 
				AND b.roleid = 2   							  
				AND b.statusID <> 101 --exclude REJECT bets	   
				AND ISNULL(b.isBA,0) = 0					 
			GROUP BY  b.winlostdate, b.currency,  b.statusID  
		END		 
	END 

	IF @isGetOldData = 1 
	BEGIN
		SELECT @MaxOldDate = MAX(winlostdate) FROM bodb_Licensee.dbo.rptLicWinloss_old (NOLOCK)
		IF @TDate >= @MaxOldDate
			SET @TempToDate = @MaxOldDate
		ELSE
			SET  @TempToDate = @TDate
	
		--exclude BA tickets
		IF @product = 0  --sportbook
		BEGIN
			INSERT INTO #SAgWinLoss  
			SELECT 
				b.winlostdate,  
				b.currency,
				SUM(b.turnover),     
				SUM(b.winlost),  																							
				SUM ( playercomm) as PlayerComm,
				SUM(CASE WHEN b.sporttype = 0  THEN b.awinlost ELSE 0 END) AS LicenseeWL,
				SUM(b.comm) as LicenseeComm,
				- SUM(CASE WHEN b.sporttype = 0 THEN b.awinlost + b.winlost ELSE 0 END) AS Company,
				SUM(b.betcount)				
			FROM 
				bodb_Licensee.dbo.rptLicWinloss_old b WITH ( NOLOCK )  
				INNER JOIN @TempAg ag ON ag.custid = b.recommend
				INNER JOIN @sporttype t ON b.sporttype = t.sporttype
			WHERE  
				b.Winlostdate BETWEEN @FDate AND @TempToDate  
				AND b.roleid = 2   
				AND b.statusID <> 101 --exclude REJECT bets
				AND ISNULL(b.isBA,0) = 0
			GROUP BY  b.winlostdate,   b.currency, b.statusID
		END
	
	END

	CREATE NONCLUSTERED INDEX IDX_Index_SA ON #SAgWinLoss(Currency)	   
	SELECT * INTO #TempExchange 									   
	FROM Lrf_f_GetExchangeDaily(@FDate,@TDate)						   
	WHERE exchangeid IN (SELECT DISTINCT Currency FROM #SAgWinLoss)	   
	SELECT   														   
	   w.winlostdate,  										 
	   Currency,   														 
	   (CASE WHEN Currency = 51 THEN @VNDCurrencyName ELSE e.Code END) CurrencyName,								 
	   SUM(TurnOver)	AS Turnover,   
	   SUM(Betcount)	AS Betcount, 										 
	   SUM(Winlost)		AS MemberWinLoss,  										 
	   SUM(PlayerComm)	AS MemberComm,
	   SUM(LicenseeWL)	AS LicenseeWL,		   
	   SUM(LicenseeComm)AS LicenseeComm,	
	   SUM(Company)		AS Company,	
	   --Base currency
	   (	   
	   CASE WHEN(@baseCurrency = 6)			
	   THEN
			CAST(SUM(Company * e.EUR) AS MONEY)     
	   ELSE
			CAST(SUM(Company * e.USD) AS MONEY)     
	   END				
	   ) AS CompanyBaseCurrency
	FROM													  
		#SAgWinLoss w   									  
		INNER JOIN  #TempExchange e							  
		ON w.winlostdate = e.ExchangeDate AND w.currency = e.ExchangeId
	GROUP BY   														   
		Currency													   
		,w.winlostdate											   
		,CASE WHEN Currency = 51 THEN @VNDCurrencyName ELSE e.Code END												   
	ORDER BY WinlostDate  													   

	DROP TABLE #TempExchange												   
	DROP TABLE #SAgWinLoss   

GO
GRANT EXECUTE ON [dbo].[IMS_Rpt_LicenseeSettlement] TO [bodbAdminNet]
--GO
--GRANT VIEW DEFINITION ON [dbo].[IMS_Rpt_LicenseeSettlement] TO [vnteam] 