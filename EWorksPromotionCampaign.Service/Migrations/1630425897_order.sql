IF (OBJECT_ID('tbl_generated_codes', 'U') is not null)
BEGIN
	DROP TABLE tbl_generated_codes;
END
GO

IF OBJECT_ID('tbl_hashed_codes', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_hashed_codes]
        (
            [id]                    bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [hashed_code]           varchar(100)        UNIQUE NOT NULL,
            [campaign_id]           bigint              NULL,
            [user_id]               bigint             NULL references tbl_users (id),
            [created_at]            datetime           default (GETUTCDATE()),
            [updated_at]            datetime           NULL
        ) ON [PRIMARY]
    end
go

IF (OBJECT_ID('usp_get_available_code') is not null)
BEGIN
	drop procedure usp_get_available_code;
END
GO

CREATE PROCEDURE usp_get_available_code (
	@user_id bigint,
	@campaign_id bigint
)
AS
BEGIN
       DECLARE @available_hashed_code VARCHAR(100);

       SELECT TOP(1) 
            @available_hashed_code = hashed_code
        FROM tbl_hashed_codes WHERE [user_id] IS NULL AND updated_at = NULL;
       IF @available_hashed_code = NULL
        BEGIN
            SELECT TOP(1) 
                @available_hashed_code = hashed_code
            FROM tbl_hashed_codes WHERE [user_id] IS NULL AND updated_at = NULL AND campaign_id = @campaign_id;
        END

     SELECT @available_hashed_code;
END
GO


IF OBJECT_ID('tbl_payments', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_payments]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [order_id]             bigint             NOT NULL references tbl_orders (id),
            [amount]               decimal(18,2)      NOT NULL,
            [description]          varchar(400)       NULL,
            [gateway_response]     varchar(400)       NOT NULL,
            [gateway_reference]    varchar(100)       NOT NULL,
            [status]               bit                NOT NULL,
            [additional_info]      varchar(450)       NULL,
            [transaction_date]     datetime           NULL,
            [created_at]           datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go


IF (OBJECT_ID('usp_create_new_order') is not null)
BEGIN
	drop procedure usp_create_new_order;
END
GO

CREATE PROCEDURE usp_create_new_order (
	@user_id bigint,
	@amount decimal(18,2),
	@campaign_id bigint
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @existing_user_id BIGINT;
    DECLARE @existing_campaign_id BIGINT;
    DECLARE @existing_campaign_min_amount DECIMAL(18,2);
    DECLARE @existing_campaign_max_amount DECIMAL(18,2);
    DECLARE @order_id BIGINT;
    DECLARE @available_code VARCHAR(100);

    SELECT
        @existing_user_id = id
    FROM tbl_users WHERE id = @user_id;

    IF @existing_user_id IS NULL
    BEGIN
        SELECT '04: User not found' AS RESULT;
        RETURN
    END

    SELECT
        @existing_campaign_id = id,
        @existing_campaign_min_amount = min_entry_amount,
        @existing_campaign_max_amount = max_entry_amount
    FROM tbl_campaigns WHERE id = @campaign_id;

    IF @existing_campaign_id IS NULL
    BEGIN
        SELECT '04: Campaign not found' AS RESULT;
        RETURN
    END

    IF @amount < @existing_campaign_min_amount OR @amount > @existing_campaign_max_amount
    BEGIN
        SELECT '05: Amount must be between '+CAST(@existing_campaign_min_amount AS VARCHAR(20))+' and '+CAST(@existing_campaign_max_amount AS VARCHAR(20))+'' AS RESULT;
        RETURN
    END

    SELECT TOP(1) 
        @available_code = hashed_code
    FROM tbl_hashed_codes WHERE [user_id] IS NULL AND updated_at IS NULL;
    IF @available_code IS NULL
    BEGIN
        SELECT TOP(1) 
            @available_code = hashed_code
        FROM tbl_hashed_codes WHERE [user_id] IS NULL AND updated_at IS NULL AND campaign_id = @campaign_id;
    END

    IF @available_code IS NULL
    BEGIN
        SELECT '05: No code available at the moment, Please try again later.' AS RESULT;
        RETURN
    END
    ELSE
    BEGIN
        DECLARE @reference VARCHAR(20) = 'WC-'+FORMAT(getdate(), 'Hmmss')+''+CAST(@existing_user_id as varchar(20))+'';
        INSERT INTO tbl_orders([user_id], amount, campaign_id, [status], reference) VALUES(@existing_user_id, @amount, @existing_campaign_id, 'created', @reference)
        SET @order_id = (SELECT SCOPE_IDENTITY());

        SELECT '00: '+CAST(@order_id as varchar(20))+' : '+@reference+'' AS RESULT;
    END
END
GO

IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_payments')
                AND NAME = 'payment_status')
    begin
        ALTER TABLE [dbo].[tbl_payments]
            ADD
                [payment_status] varchar(15) NOT NULL
    end
go

IF (OBJECT_ID('usp_create_new_payment') is not null)
BEGIN
	drop procedure usp_create_new_payment;
END
GO

CREATE PROCEDURE usp_create_new_payment (
    @order_id bigint,
    @user_email varchar(100),
	@amount decimal(18,2),
	@description varchar(400),
    @gateway_response varchar(400),
    @gateway_reference varchar(100),
    @status bit,
    @payment_status varchar(15),
    @transaction_date datetime = null,
    @additional_info varchar(450) = null
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @existing_order_id BIGINT;
    DECLARE @existing_order_amount DECIMAL(18,2);
    DECLARE @existing_order_user_id BIGINT;
    DECLARE @existing_order_status VARCHAR(15);
    DECLARE @existing_user_email varchar(100);
    DECLARE @existing_user_id bigint;
    DECLARE @assigned_code  VARCHAR(100);
    DECLARE @payment_id BIGINT;
    DECLARE @existing_campaign_id BIGINT;

    SELECT
        @existing_order_id = id,
        @existing_order_amount = amount,
        @existing_order_user_id = [user_id],
        @existing_order_status = [status],
        @existing_campaign_id = campaign_id
    FROM tbl_orders WHERE id = @order_id;

    SELECT 
        @existing_user_email = email,
        @existing_user_id = id
    FROM tbl_users WHERE email = @user_email;

    IF @existing_user_email IS NULL
    BEGIN
        SELECT '04: User not found';
        RETURN
    END

    IF @existing_order_id IS NULL
    BEGIN
        SELECT '04: Order not found';
        RETURN
    END
    IF @amount != @existing_order_amount
    BEGIN
        SELECT '05: Invalid amount';
        RETURN
    END
    IF @existing_order_user_id != @existing_user_id
    BEGIN
        SELECT '05: Invalid user';
        RETURN
    END
    IF @existing_order_status = 'completed'
    BEGIN
        SELECT '05: Order has already been completed';
        RETURN
    END

    IF @payment_status = 'success'
    BEGIN
        BEGIN TRY
        BEGIN TRAN

        SELECT TOP(1) 
            @assigned_code = hashed_code
        FROM tbl_hashed_codes WHERE [user_id] IS NULL AND updated_at IS NULL;
        IF @assigned_code IS NULL
        BEGIN
            SELECT TOP(1) 
                @assigned_code = hashed_code
            FROM tbl_hashed_codes WHERE [user_id] IS NULL AND updated_at IS NULL AND campaign_id = @existing_campaign_id;
        END

        IF @assigned_code IS NOT NULL
        BEGIN
            UPDATE tbl_hashed_codes SET [user_id] = @existing_order_user_id, updated_at = GETUTCDATE() WHERE hashed_code = @assigned_code;
        END

        INSERT INTO tbl_payments(order_id, amount, [description], [gateway_response], gateway_reference, [status], payment_status, additional_info) 
        VALUES(@order_id, @amount, @description, @gateway_response, @gateway_reference, @status, @payment_status, @additional_info)
        SET @payment_id = (SELECT SCOPE_IDENTITY());

        UPDATE tbl_orders SET [status] = 'completed', updated_at = GETUTCDATE() WHERE id = @existing_order_id;

        COMMIT TRAN
            SELECT '00: '+CAST(@payment_id as varchar(20))+'';
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION
            RETURN ERROR_MESSAGE()
        END CATCH 
    END

    ELSE
    BEGIN
        INSERT INTO tbl_payments(order_id, amount, [description], [gateway_response], gateway_reference, [status], payment_status, additional_info) 
            VALUES(@order_id, @amount, @description, @gateway_response, @gateway_reference, @status, @payment_status, @additional_info)
        SET @payment_id = (SELECT SCOPE_IDENTITY());
        UPDATE tbl_orders SET [status] = 'failed', updated_at = GETUTCDATE() WHERE id = @existing_order_id;

        SELECT '05: '+@gateway_response+'';
    END
END
GO

IF (OBJECT_ID('usp_get_user_by_email') is not null)
    BEGIN
        drop procedure usp_get_user_by_email;
    END
GO

create procedure usp_get_user_by_email @email varchar(50)
as
    SELECT Top 1 *
    FROM tbl_users(nolock)
    where email = @email;
GO