IF OBJECT_ID('tbl_users', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_users]
        (
            [id]                    bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [first_name]            varchar(50)        NOT NULL,
            [middle_name]           varchar(50)        NULL,
            [last_name]             varchar(50)        NOT NULL,
            [phone]                 varchar(50) unique NOT NULL,
            [email]                 varchar(50) unique NOT NULL,
            [address]               varchar(100)       NULL,
            [date_of_birth]         datetime           NOT NULL,
            [is_phone_verified]     bit                NOT NULL default 0,
            [is_email_verified]     bit                NOT NULL default 0,
            [is_deactivated]        bit                NOT NULL default 0,
            [is_disabled]           bit                NOT NULL default 0,
            [locked_out_enabled]    bit                NOT NULL default 0,
            [access_failed_count]   int                NOT NULL default 0,
            [disabled_comment]      text               NULL,
            [disabled_at]           datetime           NULL,
            [password_hash]         nvarchar(100)      NOT NULL,
            [password_salt]         nvarchar(100)      NOT NULL,
            [created_at]            datetime           default (GETUTCDATE()),
            [locked_out_at]         datetime           NULL,
            [updated_at]            datetime           NULL
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_campaigns', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_campaigns]
        (
            [id]                   bigint               NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [name]                 varchar(100)         NOT NULL,
            [start_date]           datetime             NOT NULL,
            [end_date]             datetime             NOT NULL,
            [min_entry_amount]     decimal(18,2)        NOT NULL,
            [max_entry_amount]     decimal(18,2)        NOT NULL,
            [status]               varchar(15)          NOT NULL,
            [created_at]           datetime             default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_generated_codes', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_generated_codes]
        (
            [id]                    bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [hashed_code]           varchar(50) UNIQUE NOT NULL,
            [campaign_id]           bigint            references tbl_campaigns (id),
            [user_id]               bigint             NULL references tbl_users (id),
            [created_at]            datetime           default (GETUTCDATE()),
            [updated_at]            datetime           NULL
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_blacklists', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_blacklists]
        (
            [id]                    bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [phone_number]          varchar(14)         NOT NULL,
            [created_at]            datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_campaign_rewards', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_campaign_rewards]
        (
            [id]                    bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [campaign_id]           bigint            references tbl_campaigns (id),
            [winning_type]          varchar(15)        NOT NULL,
            [campaign_type]          varchar(15)       NOT NULL,
            [start_mode]           varchar(15)          NOT NULL,
            [schedule_type]          varchar(15)          NOT NULL,
            [amount]                decimal(18,2)       NOT NULL,
            [start_date]           datetime             NOT NULL,
            [end_date]             datetime             NOT NULL,
            [status]               varchar(15)          NOT NULL,
            [number_of_winners]     bigint              NOT NULL,
            [schedule_winning_rule]   varchar(15)       NOT NULL,
            [nth_subscriber_value]    int                 NULL,
            [created_at]            datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_campaign_winning_retries', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_campaign_winning_retries]
        (
            [id]                    bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [campaign_id]           bigint            references tbl_campaigns (id),
            [winning_type]          varchar(15)        NOT NULL,
            [retry_for]             int             NOT NULL,
            [created_at]            datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_campaign_winning_sms', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_campaign_winning_sms]
        (
            [id]                    bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [campaign_id]           bigint            references tbl_campaigns (id),
            [winning_type]          varchar(15)        NOT NULL,
            [sms]                   varchar(256)       NOT NULL,
            [created_at]            datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_winnings', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_winnings]
        (
            [id]                    bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [telco_code]            varchar(20)        NULL,
            [phone_number]          varchar(14)        NOT NULL,
            [amount]                decimal(18,2)      NOT NULL,
            [date_won]              datetime           NOT NULL,
            [reference]             varchar(50)        NULL,
            [winning_type]          varchar(15)        NULL,
            [campaign_reward_id]    bigint      references tbl_campaign_rewards (id),
            [status]                varchar(15)        NOT NULL,
            [user_id]               bigint             NOT NULL references tbl_users (id),
            [created_at]            datetime           default (GETUTCDATE()),
            [updated_at]            datetime           NULL
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_vendor_airtime_requests', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_vendor_airtime_requests]
        (
            [id]                    bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [reference]             varchar(100)       NULL,
            [user_id]               bigint             NOT NULL references tbl_users (id),
            [sms]                   varchar(256)       NOT NULL,
            [created_at]            datetime           default (GETUTCDATE()),
            [updated_at]            datetime           NULL
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_configurations', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_promo_configurations]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [key]                  int                NOT NULL,
            [value]                nvarchar(1024)      NOT NULL,
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_subscribers', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_subscribers]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [hashed_code]          varchar(100)                NOT NULL,
            [plain_code]           varchar(100)     NULL,
            [full_name]            varchar(100)       NULL,
            [phone]                varchar(14)        NULL,
            [telco]                varchar(20)        NULL,
            [date_subscribed]      datetime           NOT NULL,
            [status]               varchar(15)        NOT NULL,
            [campaign_id]          bigint             references tbl_campaigns (id),
            [created_at]            datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_subscription_requests', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_subscription_requests]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [reward_code]          nvarchar(100)      NOT NULL,
            [message]              nvarchar(1024)     NULL,
            [phone]                varchar(14)        NULL,
            [telco]                varchar(20)        NULL,
            [status]                varchar(15)       NULL,
            [created_at]            datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_raffle_reward_types', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_raffle_reward_types]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [start_date]           datetime             NOT NULL,
            [end_date]             datetime             NOT NULL,
            [number_of_winners]    int                  NOT NULL,
            [amount]               decimal(18,2)        NOT NULL,
            [status]               varchar(15)       NULL,
            [created_at]           datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_raffle_reward_types', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_raffle_reward_types]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [start_date]           datetime             NOT NULL,
            [end_date]             datetime             NOT NULL,
            [number_of_winners]    int                  NOT NULL,
            [amount]               decimal(18,2)        NOT NULL,
            [status]               varchar(15)       NULL,
            [created_at]           datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_raffle_reward_types', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_raffle_reward_types]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [start_date]           datetime             NOT NULL,
            [end_date]             datetime             NOT NULL,
            [number_of_winners]    int                  NOT NULL,
            [amount]               decimal(18,2)        NOT NULL,
            [status]               varchar(15)       NULL,
            [created_at]           datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_raffle_winnings', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_raffle_winnings]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [user_id]               bigint             NOT NULL references tbl_users (id),
            [amount_won]               decimal(18,2)        NOT NULL,
            [status]               varchar(15)       NULL,
            [created_at]           datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go


IF OBJECT_ID('tbl_cash_requests', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_cash_requests]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [user_id]              bigint             NOT NULL references tbl_users (id),
            [amount]               decimal(18,2)      NOT NULL,
            [reference]            varchar(100)       NULL,
            [status]               varchar(15)        NOT NULL,
            [external_reference]   varchar(100)       NOT NULL,
            [external_response_message] text          NULL,
            [created_at]           datetime           default (GETUTCDATE()),
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_orders', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_orders]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [user_id]              bigint             NOT NULL references tbl_users (id),
            [amount_paid]               decimal(18,2)      NOT NULL,
            [campaign_id]          bigint             references tbl_campaigns (id),
            [status]               varchar(15)        NOT NULL,
            [reference]             varchar(100)      NOT NULL,
            [created_at]           datetime           default (GETUTCDATE()),
            [updated_at]            datetime           NULL
        ) ON [PRIMARY]
    end
go