IF (OBJECT_ID('usp_create_new_campaign') is not null)
BEGIN
	drop procedure usp_create_new_campaign;
END
GO

create procedure usp_create_new_campaign
	@name varchar(100),
	@start_date datetime,
	@end_date datetime,
	@min_entry_amount decimal(18,2),
	@max_entry_amount decimal(18,2),
	@status varchar(15)
as
	INSERT into tbl_campaigns([name], [start_date], [end_date], [min_entry_amount], [max_entry_amount], [status] ) VALUES
	(@name, @start_date, @end_date, @min_entry_amount, @max_entry_amount, @status) SELECT SCOPE_IDENTITY();
GO

IF (OBJECT_ID('usp_get_campaign_by_name') is not null)
BEGIN
	drop procedure usp_get_campaign_by_name;
END
go

create procedure usp_get_campaign_by_name
	@name varchar (100)
as
SELECT Top 1 * FROM tbl_campaigns(nolock) where [name] = @name;
GO

IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_campaigns')
                AND NAME = 'updated_at')
    begin
        ALTER TABLE [dbo].[tbl_campaigns]
            ADD
                [updated_at] datetime NULL
    end
go

IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_campaign_rewards')
                AND NAME = 'updated_at')
    begin
        ALTER TABLE [dbo].[tbl_campaign_rewards]
            ADD
                [updated_at] datetime NULL
    end
go

IF (OBJECT_ID('usp_create_new_campaign_reward') is not null)
BEGIN
	drop procedure usp_create_new_campaign_reward;
END
GO

create procedure usp_create_new_campaign_reward
	@campaign_id bigint,
	@winning_type varchar(15),
	@campaign_type varchar(15),
	@start_mode varchar(15),
	@schedule_type varchar(15),
	@amount decimal(18,2),
	@start_date datetime,
	@end_date datetime,
	@status varchar(15),
	@number_of_winners bigint,
	@schedule_winning_rule varchar(50),
	@nth_subscriber_value int = null
as
	INSERT into tbl_campaign_rewards(campaign_id, winning_type, campaign_type, start_mode, schedule_type, amount, [start_date], end_date, [status], number_of_winners, schedule_winning_rule, nth_subscriber_value) VALUES
	(@campaign_id, @winning_type, @campaign_type, @start_mode, @schedule_type, @amount, @start_date, @end_date, @status, @number_of_winners, @schedule_winning_rule, @nth_subscriber_value) SELECT SCOPE_IDENTITY();
GO

IF (OBJECT_ID('usp_fetch_campaign_by_id') is not null)
    BEGIN
        drop procedure usp_fetch_campaign_by_id;
    END
GO

create procedure usp_fetch_campaign_by_id 
@id bigint
as
    SELECT *
    FROM tbl_campaigns(nolock)
    WHERE Id = @id;
GO

IF (OBJECT_ID('usp_get_campaign_rewards_by_campaign_id') is not null)
    BEGIN
        drop procedure usp_get_campaign_rewards_by_campaign_id;
    END
GO

create procedure usp_get_campaign_rewards_by_campaign_id 
@campaign_id bigint
as
    SELECT *
    FROM tbl_campaign_rewards(nolock)
    WHERE campaign_id = @campaign_id;
GO

IF (OBJECT_ID('usp_get_campaign_rewards') is not null)
    BEGIN
        drop procedure usp_get_campaign_rewards;
    END
GO

create procedure usp_get_campaign_rewards 
as
    SELECT *
    FROM tbl_campaign_rewards(nolock);
GO

IF (OBJECT_ID('usp_fetch_campaigns') is not null)
    BEGIN
        drop procedure usp_fetch_campaigns;
    END
GO

create procedure usp_fetch_campaigns 
as
    SELECT *
    FROM tbl_campaigns(nolock);
GO

IF (OBJECT_ID('usp_fetch_campaign_reward_by_id') is not null)
    BEGIN
        drop procedure usp_fetch_campaign_reward_by_id;
    END
GO

create procedure usp_fetch_campaign_reward_by_id 
@id bigint
as
    SELECT *
    FROM tbl_campaign_rewards(nolock)
    WHERE id = @id;
GO

IF (OBJECT_ID('usp_update_campaign') is not null)
    BEGIN
        drop procedure usp_update_campaign;
    END
GO

create procedure usp_update_campaign  
    @id bigint,
	@name varchar(100),
	@start_date datetime,
	@end_date datetime,
	@min_entry_amount decimal(18,2),
	@max_entry_amount decimal(18,2)
as
UPDATE tbl_campaigns
SET [name] = @name,
    [start_date]  = @start_date,
    end_date = @end_date,
    min_entry_amount = @min_entry_amount,
    max_entry_amount = @max_entry_amount,
	updated_at = getdate()
WHERE id = @id
GO

IF (OBJECT_ID('usp_update_campaign_status') is not null)
    BEGIN
        drop procedure usp_update_campaign_status;
    END
GO

create procedure usp_update_campaign_status  
    @id bigint,
	@status varchar(15)
as
    UPDATE tbl_campaigns
    SET [status] = @status
WHERE id = @id
GO

IF (OBJECT_ID('usp_update_campaign_reward_status') is not null)
    BEGIN
        drop procedure usp_update_campaign_reward_status;
    END
GO

create procedure usp_update_campaign_reward_status  
    @id bigint,
	@status varchar(15)
as
    UPDATE tbl_campaign_rewards
    SET [status] = @status
WHERE id = @id
GO

IF EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_campaign_rewards')
                AND NAME = 'schedule_winning_rule')
    begin
        ALTER TABLE [dbo].[tbl_campaign_rewards]
            ALTER COLUMN
                [schedule_winning_rule] varchar(50) NOT NULL
    end
go