IF (OBJECT_ID('usp_process_instant_winning') is not null)
    BEGIN
        drop procedure usp_process_instant_winning;
    END
GO

CREATE PROCEDURE usp_process_instant_winning (
    @campaign_id int,
    @winning_type int,
    @winning_status int,
    @hashed_code nvarchar(256)
)
AS
SET NOCOUNT ON;
BEGIN
	DECLARE
	@request_time datetime = GETDATE(),
	@has_won INT = 0

	DECLARE
	@today as datetime = GETDATE(),
	@schedule_type INT,
	@start_time DATETIME, 
	@end_time DATETIME,
	@expected_winners INT,
	@current_hour_start DATETIME = DATEADD(HOUR, DATEDIFF(HOUR, 0, @request_time), '00:00'),
	@current_hour_end DATETIME = DATEADD(HOUR, DATEDIFF(HOUR, 0, @request_time), '00:59'),
	@current_day_start DATETIME,
	@current_day_end DATETIME,
	@current_winners_count INT,
	@win_amount DECIMAL(18,2) = 0,
	@campaign_reward_id INT = 0,
	@schedule_winning_rule INT,
	@random_number INT,
	@subscriber_id INT = 0,
	@telco	VARCHAR(20),
	@phone_number VARCHAR(14),
	@nth_request INT = 0,
	@every_nth_request INT = 0

	SELECT 
			@start_time = DATEADD(day, DATEDIFF(day, 0, GetDate()), CONVERT(TIME, [start_date])), 
			@end_time = DATEADD(day, DATEDIFF(day, 0, GetDate()), CONVERT(TIME, end_date)),
			@schedule_type = schedule_type,
			@expected_winners = number_of_winners,
			@win_amount = amount,
			@campaign_reward_id = id,
			@schedule_winning_rule = schedule_winning_rule,
			@every_nth_request = nth_subscriber_value
	FROM tbl_campaign_rewards 
	WHERE campaign_id = @campaign_id
	AND [start_date] <= @today AND end_date >= @request_time
	AND DATEADD(day, DATEDIFF(day, 0, GetDate()), '') <= @request_time 
	AND DATEADD(day, DATEDIFF(day, 0, GetDate()), '') >= @request_time
	AND winning_type = @winning_type
	AND [status] = @winning_status

	IF @schedule_type IS NULL
	BEGIN
		SELECT 'Configuration Unavailable'
		return
	END

	IF @schedule_winning_rule = 'AnyRandomUserWins' -- FirstComeFirstWin, AnyRandomUserWins, EveryNthRequest
	BEGIN
		SET @random_number = cast(rand()*1000 as int) --random number between 0 and 1000

		IF @random_number % 2 = 1
		BEGIN
			SELECT 'So close, please try again'
			return
		END
	END

	IF @schedule_type = 'Hourly'
	BEGIN
		-- count number of winners within hour
		SELECT @current_winners_count = COUNT(1) FROM tbl_winnings 
		WHERE @current_hour_start <=  date_won AND @current_hour_end >= date_won
		AND winning_type = @winning_type

		IF @every_nth_request = 'EveryNthSubscriber' -->> Every Nth request
		BEGIN
			SELECT @nth_request = COUNT(1) FROM tbl_subscribers 
			WHERE @current_hour_start <= date_subscribed AND @current_hour_end >= date_subscribed

			IF @nth_request % @every_nth_request <> 1
			BEGIN
				SELECT 'Winning Unavailable'
				return
			END
		END
	END

	IF @schedule_type = 'Daily'
	BEGIN
		-- count number of winners today
		SELECT @current_winners_count = COUNT(1) FROM tbl_winnings 
		WHERE @start_time <=  date_won AND @end_time >= date_won
		AND winning_type = @winning_type

		IF @schedule_winning_rule = 'EveryNthSubscriber' -->> Every Nth request
		BEGIN
			SELECT @nth_request = COUNT(1) FROM tbl_subscribers
			WHERE @start_time <= date_subscribed AND @end_time >= date_subscribed

			if @nth_request % @every_nth_request <> 1
			begin
				SELECT 'Winning Unavailable'
				return
			end
		END

	END

	IF @current_winners_count < @expected_winners
	BEGIN
		SELECT 
			@subscriber_id = id,
			@telco = telco,
			@phone_number = phone
		FROM tbl_subscribers
		WHERE hashed_code = @hashed_code;

		BEGIN TRY
			BEGIN TRAN
				INSERT INTO tbl_winnings(subscriber_id,telco_code,phone_number,amount,date_won,reference,winning_type,campaign_reward_id,status) 
				SELECT 
					@subscriber_id,
					@telco,
					@phone_number,
					@win_amount,
					GetDate(),
					'',
					@winning_type,
					@campaign_reward_id,
					'Pending' --pending 

				SET @has_won = 1

			COMMIT TRAN
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION;	
			SELECT 'Winning Unavailable'
			Return
		END CATCH
	END
	
	ELSE
	BEGIN
		SELECT 'Winning Unavailable'
		Return
	END

	IF @has_won = 0
	BEGIN
		SELECT 'So close, please try again'
		Return
	END

	Select 'PARAMS:' + 
			cast(@subscriber_id as varchar) + ':' 
			+ cast(@campaign_reward_id as varchar) as WinningParams	
END
GO
