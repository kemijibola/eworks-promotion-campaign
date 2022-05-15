IF OBJECT_ID('tbl_admin_users', 'U') IS NULL
    begin
        CREATE TABLE [dbo].[tbl_admin_users]
        (
            [id]                  bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [first_name]          varchar(50)        NOT NULL,
            [last_name]           varchar(50)        NOT NULL,
            [phone]               varchar(50) unique NOT NULL,
            [email]               varchar(50) unique NOT NULL,
            [role_id]             int, 
            [password_hash]       nvarchar(100)      NOT NULL,
            [password_salt]       nvarchar(100)      NOT NULL,
            [access_failed_count] int,
            [locked_out]          bit      default (0),
            [locked_out_date]     datetime           NULL,
            [lock_out_enabled]    bit      default (1),
            [status_updated_by]   bigint  FOREIGN KEY REFERENCES dbo.tbl_admin_users NULL,
            [is_disabled]         bit      default (0),
            [status]              bit      default (0),
            [is_first_login]      bit      default(1),
            [created_at]          datetime default (getdate()),
            [status_comment]       varchar(350)       NULL,
            [status_updated_at]    datetime     NULL,
            [updated_at]          datetime           NULL
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_admin_roles', 'U') IS NULL
    begin
        CREATE TABLE [dbo].[tbl_admin_roles]
        (
            [id]                  bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [role_name]           varchar(50)        NOT NULL,
            [role_description]    varchar(50)        NULL,
            [status]              bit                default (0),
            [status_updated_by]   bigint     FOREIGN KEY REFERENCES dbo.tbl_admin_users NULL,
            [created_at]          datetime           default (getdate()),
            [updated_at]          datetime           NULL,
            [status_updated_at]         datetime     NULL,
            [status_comment]       varchar(350)       NULL
        ) ON [PRIMARY]
    end
go


IF OBJECT_ID('tbl_admin_permissions', 'U') IS NULL
    begin
        CREATE TABLE [dbo].[tbl_admin_permissions]
        (
            [id]                        bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [permission_name]           varchar(50)        NOT NULL,
            [permission_description]    varchar(50)        NULL,
            [status]            bit     default (0),
            [status_updated_by]         bigint            FOREIGN KEY REFERENCES dbo.tbl_admin_users NULL,
            [created_at]          datetime           default (getdate()),
            [updated_at]          datetime           NULL,
            [status_updated_at]         datetime     NULL,
            [status_comment]       varchar(350)       NULL
        ) ON [PRIMARY]
    end
go

IF OBJECT_ID('tbl_admin_role_permissions', 'U') IS NULL
    begin
        CREATE TABLE [dbo].[tbl_admin_role_permissions]
        (
            [id]                bigint  NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [role_id]           int       NOT NULL,
            [permission_id]    int        NOT NULL,
            [created_at]          datetime                 default (getdate()),
            [updated_at]          datetime                 NULL
        ) ON [PRIMARY]
    end
go

IF (OBJECT_ID('usp_get_admin_user_by_email') is not null)
    BEGIN
        drop procedure usp_get_admin_user_by_email;
    END
GO

create procedure usp_get_admin_user_by_email @email varchar(50)
as
    SELECT Top 1 *
    FROM tbl_admin_users(nolock) ta
    where ta.email = @email;
GO

IF (OBJECT_ID('usp_create_new_admin_role') is not null)
    BEGIN
        drop procedure usp_create_new_admin_role;
    END
GO

create procedure usp_create_new_admin_role @role_name varchar(50),
                                        @role_description varchar(50)
as
    INSERT INTO tbl_admin_roles (role_name, role_description)
    VALUES (@role_name, @role_description)
    SELECT SCOPE_IDENTITY();
GO

IF (OBJECT_ID('usp_fetch_role_by_id') is not null)
    BEGIN
        drop procedure usp_fetch_role_by_id;
    END
GO

create procedure usp_fetch_role_by_id 
@id bigint
as
    SELECT *
    FROM tbl_admin_roles(nolock)
    WHERE Id = @id;
GO

IF (OBJECT_ID('usp_fetch_role_by_name') is not null)
    BEGIN
        drop procedure usp_fetch_role_by_name;
    END
GO

create procedure usp_fetch_role_by_name 
@role_name varchar(50)
as
    SELECT *
    FROM tbl_admin_roles(nolock)
    WHERE role_name = @role_name;
GO

IF (OBJECT_ID('usp_get_role_permissions_by_role_id') is not null)
    BEGIN
        drop procedure usp_get_role_permissions_by_role_id;
    END
GO

create procedure usp_get_role_permissions_by_role_id 
@role_id bigint
as

    SELECT *
    FROM tbl_admin_permissions (nolock)
    WHERE Id IN (SELECT permission_id from tbl_admin_role_permissions where role_id = @role_id);
GO

IF (OBJECT_ID('usp_get_new_admin_user_phone_email_validation') is not null)
    BEGIN
        drop procedure usp_get_new_admin_user_phone_email_validation;
    END
GO

create procedure usp_get_new_admin_user_phone_email_validation 
    @phone varchar(50),
    @email varchar(50)
as
    SELECT Top 1 *
    FROM tbl_admin_users(nolock)
    where phone=@phone or email=@email;
GO

IF (OBJECT_ID('usp_update_role_status') is not null)
    BEGIN
        drop procedure usp_update_role_status;
    END
GO

create procedure usp_update_role_status 
    @role_id bigint,
    @status int,
    @comment varchar(350),
    @updated_by int
as
    UPDATE tbl_admin_roles
    SET [status] = @status, status_updated_by =@updated_by, status_comment =@comment, status_updated_at = getdate()
    WHERE id = @role_id;
GO

IF (OBJECT_ID('usp_delete_role_permissions_by_role_id') is not null)
    BEGIN
        drop procedure usp_delete_role_permissions_by_role_id;
    END
GO

create procedure usp_delete_role_permissions_by_role_id 
@role_id bigint
as
    DELETE FROM tbl_admin_role_permissions where role_id = @role_id;
GO

IF (OBJECT_ID('usp_add_permission_to_role') is not null)
    BEGIN
        drop procedure usp_add_permission_to_role;
    END
GO

create procedure usp_add_permission_to_role 
@permission_id bigint,
@role_id bigint
as
    INSERT INTO tbl_admin_role_permissions(role_id,permission_id) VALUES(@role_id,@permission_id);
GO

IF (OBJECT_ID('usp_get_permissions') is not null)
    BEGIN
        drop procedure usp_get_permissions;
    END
GO

create procedure usp_get_permissions 
as
    SELECT * FROM tbl_admin_permissions(nolock);
GO

IF (OBJECT_ID('usp_get_roles') is not null)
    BEGIN
        drop procedure usp_get_roles;
    END
GO

create procedure usp_get_roles 
as
    SELECT * FROM tbl_admin_roles(nolock);
GO

IF (OBJECT_ID('usp_get_admin_user_by_identifier') is not null)
    BEGIN
        drop procedure usp_get_admin_user_by_identifier;
    END
GO

create procedure usp_get_admin_user_by_identifier
@identifier varchar (50)
as
SELECT Top 1 * FROM tbl_admin_users(nolock) where email = @identifier OR phone = @identifier;
GO

IF (OBJECT_ID('usp_get_admin_user_by_email_phone') is not null)
BEGIN
	drop procedure usp_get_admin_user_by_email_phone;
END
go

create procedure usp_get_admin_user_by_email_phone
	@email varchar (50),
	@phone varchar(50)
as
SELECT Top 1 * FROM tbl_admin_users(nolock) where email = @email OR  phone = @phone;
GO

IF (OBJECT_ID('usp_get_admin_user_by_email') is not null)
    BEGIN
        drop procedure usp_get_admin_user_by_email;
    END
GO

create procedure usp_get_admin_user_by_email @email varchar(50)
as
    SELECT Top 1 *
    FROM tbl_admin_users (nolock)
    where email = @email;
GO

IF (OBJECT_ID('usp_update_admin_user') is not null)
    BEGIN
        drop procedure usp_update_admin_user;
    END
GO

create procedure usp_update_admin_user @first_name varchar(50),
                                         @last_name varchar(50),
                                         @phone varchar(50),
                                         @email varchar(50),
                                         @user_id bigint
as
UPDATE tbl_admin_users
SET first_name = @first_name,
    last_name  = @last_name,
    phone      = @phone,
    email      = @email
WHERE id = @user_id
GO

IF (OBJECT_ID('usp_get_admin_user_by_id') is not null)
    BEGIN
        drop procedure usp_get_admin_user_by_id;
    END
GO

create procedure usp_get_admin_user_by_id 
@id bigint
as
    SELECT Top 1 *
    FROM tbl_admin_users(nolock) where id = @id;
GO

IF (OBJECT_ID('usp_update_role_status') is not null)
    BEGIN
        drop procedure usp_update_role_status;
    END
GO

create procedure usp_update_role_status 
    @role_id bigint,
    @status int,
    @comment varchar(350),
    @updated_by int
as
    UPDATE tbl_admin_roles
    SET [status] = @status, status_updated_by =@updated_by, status_comment =@comment, status_updated_at = getdate()
    WHERE id = @role_id;
GO

IF (OBJECT_ID('usp_update_permission_status') is not null)
    BEGIN
        drop procedure usp_update_permission_status;
    END
GO

create procedure usp_update_permission_status 
    @permission_id bigint,
    @status int,
    @comment varchar(350),
    @updated_by int
as
    UPDATE tbl_admin_permissions
    SET [status] = @status, status_updated_by =@updated_by, status_comment =@comment, status_updated_at = getdate()
    WHERE id = @permission_id;
GO

IF (OBJECT_ID('usp_create_new_admin_user') is not null)
BEGIN
	drop procedure usp_create_new_admin_user;
END
GO

create procedure usp_create_new_admin_user
	@first_name varchar(50),
	@last_name varchar(50),
	@phone varchar(50),
	@email varchar(50),
    @role_id bigint,
	@password_hash nvarchar(100),
	@password_salt nvarchar(100)
as
	INSERT into tbl_admin_users(first_name, last_name, phone, email, role_id, password_hash, password_salt) VALUES
	(@first_name, @last_name, @phone, @email, @role_id,  @password_hash, @password_salt) SELECT SCOPE_IDENTITY();
GO

IF (OBJECT_ID('usp_update_admin_user_status') is not null)
    BEGIN
        drop procedure usp_update_admin_user_status;
    END
GO

create procedure usp_update_admin_user_status 
    @user_id bigint,
    @status int,
    @comment varchar(350),
    @updated_by int
as
    UPDATE tbl_admin_users
    SET [status] = @status, status_updated_by =@updated_by, status_comment =@comment, status_updated_at = getdate()
    WHERE id = @user_id;
GO