IF NOT EXISTS (SELECT *
               FROM sys.types
               WHERE is_table_type = 1 AND name = 'subscriber_airtime_winning_type')
CREATE TYPE subscriber_airtime_winning_type AS TABLE (
        id       			BIGINT,
        phone_number       	VARCHAR(14),
		date_won 			DATETIME,
		winning_type		VARCHAR(15)
    )
GO

IF NOT EXISTS (SELECT *
               FROM sys.types
               WHERE is_table_type = 1 AND name = 'subscriber_cash_winning_type')
CREATE TYPE subscriber_cash_winning_type AS TABLE (
        id       			BIGINT,
        phone_number       	VARCHAR(14),
		date_won 			DATETIME,
		winning_type		VARCHAR(15)
    )
GO

IF (OBJECT_ID('usp_new_subcription') is not null)
    BEGIN
        drop procedure usp_new_subcription;
    END
GO

CREATE PROCEDURE usp_new_subcription (
	@hashed_code   VARCHAR(100),
	@plain_code VARCHAR(100),
	@subscriber_phone VARCHAR(13),
	@subscriber_telco VARCHAR(15),
	@user_id BIGINT
)
AS
SET NOCOUNT ON;
BEGIN
	DECLARE
		@campaign_id BIGINT,
		@campaign_unavailable_message VARCHAR(300),
		@non_existent_daily_limit_message VARCHAR(300),
		@maximum_daily_limit_reached_message VARCHAR(300),
		@black_listed_subscriber_message VARCHAR(300),
		@invalid_reward_code_message VARCHAR(300),
		@used_reward_code_message VARCHAR(300),
		@daily_limit INT,
		@subscriber_phone_daily_subscription INT,
		@today as DATETIME = GETDATE(),
		@blacklisted_phone VARCHAR(15),
		@user_hashed_code VARCHAR(100),
		@used_hashed_code VARCHAR(100),
		@campaign_reward_id BIGINT,
		@daily_subscriber_airtime_winnings as subscriber_airtime_winning_type,
		@daily_subscriber_airtime_winnings_count INT,
		@daily_subscriber_cash_winnings_count INT,
		@daily_subscriber_cash_winnings as subscriber_cash_winning_type,
		@max_cash_winninng_per_day INT,
		@max_airtime_winning_per_day INT
		
	SELECT 
		@campaign_id = id
	FROM tbl_campaigns 
	WHERE [status]= 'ongoings';
	
	IF @campaign_id IS NULL
	BEGIN
		SELECT 
			@campaign_unavailable_message = (SELECT '04:' + [value]
		FROM tbl_configurations 
		WHERE [key]= 'NoActiveCampaignMessage');
		
		IF @campaign_unavailable_message IS NULL
		BEGIN
			SET @campaign_unavailable_message = '04: No campaign is active at this moment';
		END
	SELECT @campaign_unavailable_message;
	RETURN
	END
	
	SELECT 
		@daily_limit = CAST([VALUE] AS INT)
	FROM tbl_configurations 
	WHERE [key]= 'DailySubscriptionlimit';
	
	IF @daily_limit IS NULL
	BEGIN
		SELECT 
			@non_existent_daily_limit_message = (SELECT '04:' + [value]
		FROM tbl_configurations 
		WHERE [key]= 'NonExistentDailyLimitsMessage');
		
		IF @non_existent_daily_limit_message IS NULL
		BEGIN
			SET @non_existent_daily_limit_message = '04: Daily limits has not been configured';
		END
	SELECT @non_existent_daily_limit_message;
	RETURN
	END
	
	SELECT 
		@subscriber_phone_daily_subscription = COUNT(1)
	 FROM tbl_subscribers WHERE phone = @subscriber_phone AND date_subscribed = @today;
	 
	 IF @daily_limit >= @subscriber_phone_daily_subscription
	 BEGIN
		SELECT 
			@maximum_daily_limit_reached_message = (SELECT '01:' + [value]
		FROM tbl_configurations 
		WHERE [key]= 'MaximumDailyLimitReachedMessage');
		
		IF @maximum_daily_limit_reached_message IS NULL
		BEGIN
			SET @maximum_daily_limit_reached_message = '01: This phone number has reached the Maximum Daily Limit set';
		END
	SELECT @maximum_daily_limit_reached_message;
	RETURN
	END
	
	SELECT TOP(1)
		@blacklisted_phone = phone_number
	 FROM tbl_blacklists WHERE phone_number = @subscriber_phone;
	 
	IF @blacklisted_phone IS NOT NULL
	 BEGIN
		SELECT 
			@black_listed_subscriber_message = (SELECT '01:' + [value]
		FROM tbl_configurations 
		WHERE [key]= 'BlacklistedSubscriberMessage');
		
		IF @black_listed_subscriber_message IS NULL
		BEGIN
			SET @black_listed_subscriber_message = '01: For this phone number, No campaign is active at this moment';
		END
	SELECT @black_listed_subscriber_message;
	RETURN
	END
	
	SELECT TOP(1)
		@user_hashed_code = hashed_code
	 FROM tbl_generated_codes WHERE hashed_code = @hashed_code AND user_id = @user_id;
	 
	IF @user_hashed_code IS NULL
	 BEGIN
		SELECT 
			@invalid_reward_code_message = (SELECT '01:' + [value]
		FROM tbl_configurations 
		WHERE [key]= 'InvalidRewardCodeMessage');
		
		IF @invalid_reward_code_message IS NULL
		BEGIN
			SET @invalid_reward_code_message = '01: Invalid Reward code, Reward code does not exist';
		END
	SELECT @invalid_reward_code_message;
	RETURN
	END
	
	SELECT TOP(1)
		@used_hashed_code = hashed_code
	 FROM tbl_subscribers WHERE hashed_code = @hashed_code;
	 
	IF @used_hashed_code IS NOT NULL
	 BEGIN
		SELECT 
			@used_reward_code_message = (SELECT '01:' + [value]
		FROM tbl_configurations 
		WHERE [key]= 'UsedRewardCodeMessage');
		
		IF @used_reward_code_message IS NULL
		BEGIN
			SET @used_reward_code_message = '01: Reward code has already been used';
		END
	SELECT @used_reward_code_message;
	RETURN
	END
BEGIN TRY

	INSERT INTO tbl_subscribers(hashed_code, plain_code, [user_id], phone, telco, date_subscribed, status, campaign_id) VALUES(@user_hashed_code, @plain_code, @user_id, @subscriber_phone, @subscriber_telco, @today, 'Drawn', @campaign_id)
	SET @campaign_reward_id = (SELECT SCOPE_IDENTITY());
	
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	RETURN ERROR_MESSAGE()
END CATCH

	INSERT @daily_subscriber_airtime_winnings(id, phone_number, date_won, winning_type) SELECT subscriber_id, phone_number, date_won, winning_type FROM tbl_winnings 
	WHERE winning_type = 'airtime' AND phone_number = @subscriber_phone AND date_won >= DATEADD(DAY, -1, @today) AND date_won < @today
		SET @daily_subscriber_airtime_winnings_count = (SELECT COUNT(1) FROM @daily_subscriber_airtime_winnings);
	
	INSERT @daily_subscriber_cash_winnings(id, phone_number, date_won, winning_type) SELECT subscriber_id, phone_number, date_won, winning_type FROM tbl_winnings 
	WHERE winning_type = 'cash' AND phone_number = @subscriber_phone AND date_won >= DATEADD(DAY, -1, @today) AND date_won < @today
		SET @daily_subscriber_cash_winnings_count = (SELECT COUNT(1) FROM @daily_subscriber_cash_winnings);
	
	SELECT 
		@max_cash_winninng_per_day = CAST([VALUE] AS INT)
	FROM tbl_configurations 
	WHERE [key]= 'MaximumDailyCashLimit';
	
	IF @max_cash_winninng_per_day IS NULL
		SET @max_cash_winninng_per_day = 1;

	SELECT 
		@max_airtime_winning_per_day = CAST([VALUE] AS INT)
	FROM tbl_configurations 
	WHERE [key]= 'MaximumDailyAirtimeLimit';
	
	IF @max_airtime_winning_per_day IS NULL
		SET @max_airtime_winning_per_day = 3;
		
	IF @daily_subscriber_cash_winnings_count < @max_cash_winninng_per_day
	BEGIN 
		SET @max_airtime_winning_per_day = 3; -- call process instant winning
	END
	IF @daily_subscriber_airtime_winnings_count < @max_airtime_winning_per_day
	BEGIN
		SET @max_airtime_winning_per_day = 3; -- call process instant winning
	END
END
GO