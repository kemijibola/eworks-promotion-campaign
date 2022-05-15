IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_winnings')
                AND NAME = 'subscriber_id')
    begin
        ALTER TABLE [dbo].[tbl_winnings]
            ADD
                [subscriber_id] bigint 
    end
go

IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_raffle_reward_types')
                AND NAME = 'date_drawn')
    begin
        ALTER TABLE [dbo].[tbl_raffle_reward_types]
            ADD
                [date_drawn] datetime NULL 
    end
go

IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_raffle_reward_types')
                AND NAME = 'campaign_id')
    begin
        ALTER TABLE [dbo].[tbl_raffle_reward_types]
            ADD
                [campaign_id] bigint NOT NULL references tbl_campaigns (id)
    end
go

IF (OBJECT_ID('usp_create_new_campaign_raffle_reward') is not null)
BEGIN
	drop procedure usp_create_new_campaign_raffle_reward;
END
GO

create procedure usp_create_new_campaign_raffle_reward
	@campaign_id bigint,
	@start_date datetime,
	@end_date datetime,
	@number_of_winners bigint,
	@amount decimal(18,2),
    @status varchar(15)
as
BEGIN

    SET NOCOUNT ON;
    DECLARE @campaign_reward_id BIGINT;
    DECLARE @result VARCHAR(300);
    DECLARE @error_number INT;
    BEGIN TRY
	    INSERT into tbl_raffle_reward_types(campaign_id, [start_date], end_date, number_of_winners, amount, [status]) VALUES
	    (@campaign_id, @start_date, @end_date, @number_of_winners, @amount, @status) 
            SET @campaign_reward_id = (SELECT SCOPE_IDENTITY());
    SET @result = '00: '+cast(@campaign_reward_id as varchar)+'';
    END TRY
    BEGIN CATCH
		IF ERROR_NUMBER() = 547
            SET @result = '04: Campaign id not found';
        ELSE SET @result = '05: '+ERROR_MESSAGE()+''
	END CATCH
    SELECT @result;
END
GO