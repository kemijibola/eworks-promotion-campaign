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
    email      = @email,
	updated_at = getdate()
WHERE id = @user_id
GO

IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_admin_users')
                AND NAME = 'disabled_comment')
    begin
        ALTER TABLE [dbo].[tbl_admin_users]
            ADD
                [disabled_comment] varchar(350) NULL
    end
go

IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_admin_users')
                AND NAME = 'disabled_at')
    begin
        ALTER TABLE [dbo].[tbl_admin_users]
            ADD
                [disabled_at] datetime NULL
    end
go

IF NOT EXISTS(SELECT *
              FROM SYS.COLUMNS
              WHERE OBJECT_ID = OBJECT_ID('tbl_admin_users')
                AND NAME = 'disabled_by')
    begin
        ALTER TABLE [dbo].[tbl_admin_users]
            ADD
                [disabled_by] bigint  FOREIGN KEY REFERENCES dbo.tbl_admin_users NULL
    end
go

IF (OBJECT_ID('usp_update_user_disabled_status') is not null)
    BEGIN
        drop procedure usp_update_user_disabled_status;
    END
GO

create procedure usp_update_user_disabled_status 
    @user_id bigint,
    @status int,
    @comment varchar(350),
    @disabled_by int
as
    UPDATE tbl_admin_users
    SET [is_disabled] = @status, disabled_by =@disabled_by, disabled_comment =@comment, disabled_at = getdate()
    WHERE id = @user_id;
GO

--IF (OBJECT_ID('usp_create_new_admin_permission') is not null)
--    BEGIN
--        drop procedure usp_create_new_admin_permission;
--    END
--GO

--create procedure usp_create_new_admin_permission @permission_name varchar(50),
--                                        @permission_description varchar(50)
--as
--    INSERT INTO tbl_admin_permissions ([permission_name], permission_description)
--    VALUES (@permission_name, @permission_description)
--    SELECT SCOPE_IDENTITY();
--GO

--IF (OBJECT_ID('usp_fetch_permission_by_permission_name') is not null)
--    BEGIN
--        drop procedure usp_fetch_permission_by_permission_name;
--    END
--GO

--create procedure usp_fetch_permission_by_permission_name 
--@permission_name varchar(50)
--as
--    SELECT *
--    FROM tbl_admin_permissions(nolock)
--    WHERE [permission_name] = @permission_name;
--GO