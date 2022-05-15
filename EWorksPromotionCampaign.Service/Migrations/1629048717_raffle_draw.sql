IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_raffle_winnings')
                AND NAME = 'raffle_reward_type_id')
    begin
        ALTER TABLE [dbo].[tbl_raffle_winnings]
            ADD
                [raffle_reward_type_id] bigint NOT NULL
    end
go

IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_raffle_winnings')
                AND NAME = 'winning_type')
    begin
        ALTER TABLE [dbo].[tbl_raffle_winnings]
            ADD
                [winning_type] varchar(15) NOT NULL
    end
go

IF NOT EXISTS (SELECT *
               FROM sys.types
               WHERE is_table_type = 1 AND name = 'raffle_winning_type')
CREATE TYPE raffle_winning_type AS TABLE (
        id       			BIGINT,
        subscriber_phone    VARCHAR(14),
		amount_won 			DECIMAL(18,2),
		created_at			DATETIME
    )
GO

IF NOT EXISTS (SELECT *
               FROM sys.types
               WHERE is_table_type = 1 AND name = 'subscriber_type')
CREATE TYPE subscriber_type AS TABLE (
        id       			BIGINT,
        hashed_code			VARCHAR(100),
		plain_code 			VARCHAR(100),
		[winning_type]      VARCHAR(15),
		[full_name]			VARCHAR(150),
		[phone]				VARCHAR(14),
		[telco]				VARCHAR(20),
		[date_subscribed]	DATETIME,
		[status]			VARCHAR(15),
		[campaign_id]		BIGINT,
		[created_at]		DATETIME
    )
GO

IF (OBJECT_ID('usp_get_eligible_raffle_subscribers') is not null)
    BEGIN
        drop procedure usp_get_eligible_raffle_subscribers;
    END
GO

CREATE PROCEDURE usp_get_eligible_raffle_subscribers (
    @raffle_reward_id int
)
AS
SET NOCOUNT ON;
BEGIN
	DECLARE
	@raffle_winners as raffle_winning_type,
	@subscribers as subscriber_type,
	@raffle_reward_type_id bigint,
	@raffle_reward_campaign_id bigint,
	@raffle_reward_start_date datetime,
	@raffle_reward_end_date datetime,
	@raffle_reward_number_of_winners int,
	@raffle_reward_amount decimal(18,2),
	@raffle_reward_status varchar(14),
	@raffle_reward_created_at datetime

	SELECT 
		@raffle_reward_type_id = id,
		@raffle_reward_campaign_id = campaign_id,
		@raffle_reward_start_date = [start_date],
		@raffle_reward_end_date = [end_date],
		@raffle_reward_number_of_winners = number_of_winners,
		@raffle_reward_amount = amount,
		@raffle_reward_status = [status],
		@raffle_reward_created_at = created_at
	FROM tbl_raffle_reward_types
	WHERE id = @raffle_reward_id;

	IF @raffle_reward_type_id IS NULL
	BEGIN
		SELECT '04: Unable to find Raffle' AS result
		return
	END

	INSERT @subscribers(id, hashed_code, plain_code, winning_type, full_name, phone, telco, date_subscribed, [status], campaign_id, created_at)
	SELECT tbls.id, tbls.hashed_code, tbls.plain_code, tblw.winning_type, tblu.first_name + ' ' + tblu.middle_name + ' ' + tblu.last_name full_name, tbls.phone, tbls.telco, tbls.date_subscribed, tbls.[status], tbls.campaign_id, tbls.created_at 
	FROM tbl_subscribers tbls 
	INNER JOIN tbl_winnings tblw ON tbls.id = tblw.subscriber_id
	INNER JOIN tbl_users tblu ON tblu.id = tbls.[user_id]
	WHERE tbls.date_subscribed >= @raffle_reward_start_date AND tbls.date_subscribed <= @raffle_reward_end_date AND
	(tbls.[status] = 'Drawn' OR (tblw.winning_type = 'Airtime')) AND tbls.campaign_id = @raffle_reward_campaign_id;

	INSERT @raffle_winners(id, subscriber_phone, amount_won, created_at) 
		SELECT tblr.subscriber_id, tblu.phone, tblr.amount_won, tblr.created_at 
		FROM tbl_raffle_winnings tblr
		INNER JOIN tbl_subscribers tbls ON tblr.subscriber_id = tbls.id
		INNER JOIN tbl_users tblu ON tblu.id = tbls.[user_id]

	SELECT @raffle_reward_type_id id, @raffle_reward_campaign_id campaign_id, @raffle_reward_start_date [start_date],
	@raffle_reward_end_date end_date, @raffle_reward_number_of_winners number_of_winners, @raffle_reward_amount amount,
	@raffle_reward_status [status], @raffle_reward_created_at created_at;

	SELECT * FROM @raffle_winners;

	SELECT * FROM @subscribers;

END
GO

IF (OBJECT_ID('usp_update_raffle_winners') is not null)
    BEGIN
        drop procedure usp_update_raffle_winners;
    END
GO

CREATE PROCEDURE usp_update_raffle_winners (
	@raffle_reward_type_id   BIGINT,
	@raffle_reward_amount  DECIMAL(18,2),
	@winning_type		   VARCHAR(15),
	@subscribers           subscriber_type READONLY
	)
	AS
	BEGIN
		SET NOCOUNT ON;
		BEGIN TRY
		BEGIN TRAN


		UPDATE tbl_subscribers SET [status] = 'Won' WHERE id IN (SELECT
			sub.id
			FROM @subscribers AS sub)

		INSERT INTO tbl_raffle_winnings (
			subscriber_id,
			amount_won,
			raffle_reward_type_id,
			winning_type
		)
		SELECT
			sub.id as subscriber_id,
			@raffle_reward_amount,
			@raffle_reward_type_id,
			@winning_type
		FROM @subscribers AS sub;

		UPDATE tbl_raffle_reward_types SET [status] = 'Drawn', date_drawn = GETUTCDATE() WHERE id = @raffle_reward_type_id; 

	COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		RETURN ERROR_MESSAGE()
	END CATCH
	END
GO

IF (OBJECT_ID('usp_fetch_raffle_reward_by_id') is not null)
    BEGIN
        drop procedure usp_fetch_raffle_reward_by_id;
    END
GO

create procedure usp_fetch_raffle_reward_by_id 
@id bigint
as
	DECLARE
		@raffle_reward_type_id bigint,
		@raffle_reward_campaign_id bigint,
		@raffle_reward_start_date datetime,
		@raffle_reward_end_date datetime,
		@raffle_reward_number_of_winners int,
		@raffle_reward_amount decimal(18,2),
		@raffle_reward_status varchar(14),
		@raffle_reward_created_at datetime

		SELECT 
		@raffle_reward_type_id = id,
		@raffle_reward_campaign_id = campaign_id,
		@raffle_reward_start_date = [start_date],
		@raffle_reward_end_date = [end_date],
		@raffle_reward_number_of_winners = number_of_winners,
		@raffle_reward_amount = amount,
		@raffle_reward_status = [status],
		@raffle_reward_created_at = created_at
	FROM tbl_raffle_reward_types
	WHERE id = @id;

	IF @raffle_reward_type_id IS NULL
	BEGIN
		SELECT '04: Unable to find Raffle reward' AS result
		return
	END

	SELECT @raffle_reward_type_id id, @raffle_reward_campaign_id campaign_id, @raffle_reward_start_date [start_date],
	@raffle_reward_end_date end_date, @raffle_reward_number_of_winners number_of_winners, @raffle_reward_amount amount,
	@raffle_reward_status [status], @raffle_reward_created_at created_at;

GO

IF (OBJECT_ID('usp_delete_raffle_reward') is not null)
    BEGIN
        drop procedure usp_delete_raffle_reward;
    END
GO

create procedure usp_delete_raffle_reward 
@id bigint
as
	DECLARE
		@raffle_reward_type_id bigint
		SELECT @raffle_reward_type_id = id FROM tbl_raffle_reward_types WHERE id = @id;
		IF @raffle_reward_type_id IS NULL
		BEGIN
			SELECT '04: Unable to find Raffle reward'
			RETURN
		END
		DELETE FROM tbl_raffle_reward_types WHERE id = @id;
		SELECT '00: Success'
GO

IF (OBJECT_ID('usp_get_raffle_reward_winners') is not null)
    BEGIN
        drop procedure usp_get_raffle_reward_winners;
    END
GO

CREATE PROCEDURE usp_get_raffle_reward_winners (
    @raffle_reward_id int
)
AS
SET NOCOUNT ON;
BEGIN
	DECLARE
	@subscribers as subscriber_type,
	@raffle_reward_type_id bigint

	SELECT 
		@raffle_reward_type_id = id
	FROM tbl_raffle_reward_types
	WHERE id = @raffle_reward_id;

	IF @raffle_reward_type_id IS NULL
	BEGIN
		SELECT '04: Unable to find Raffle Reward' AS result
		return
	END

	INSERT @subscribers(id, hashed_code, plain_code, winning_type, full_name, phone, telco, date_subscribed, [status], campaign_id, created_at)
	SELECT tbls.id, tbls.hashed_code, tbls.plain_code, tblw.winning_type, tblu.first_name + ' ' + tblu.middle_name + ' ' + tblu.last_name full_name, tbls.phone, tbls.telco, tbls.date_subscribed, tbls.[status], tbls.campaign_id, tbls.created_at 
	FROM tbl_subscribers tbls 
	INNER JOIN tbl_raffle_winnings tblw ON tbls.id = tblw.subscriber_id
	INNER JOIN tbl_users tblu ON tblu.id = tbls.[user_id]
	WHERE tblw.raffle_reward_type_id = @raffle_reward_type_id;

	SELECT * FROM @subscribers;

END
GO