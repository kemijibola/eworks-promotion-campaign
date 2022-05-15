IF NOT EXISTS (SELECT *
               FROM sys.types
               WHERE is_table_type = 1 AND name = 'users_in_role_type')
CREATE TYPE users_in_role_type AS TABLE (
        role_id       		BIGINT,
        [user_id]       	VARCHAR(14)
    )
GO

IF (OBJECT_ID('usp_delete_role_transaction') is not null)
    BEGIN
        drop procedure usp_delete_role_transaction;
    END
GO

CREATE PROCEDURE usp_delete_role_transaction (
@role_id BIGINT
)
AS
BEGIN
SET NOCOUNT ON;
DECLARE @users_in_role AS users_in_role_type;
DECLARE @users_in_role_count INT;
DECLARE @existing_role_corporate_id BIGINT;
DECLARE @existing_role_name VARCHAR(50);

SELECT
@existing_role_name = [role_name]
FROM tbl_admin_roles WHERE id = @role_id;

IF @existing_role_name IS NULL
BEGIN
SELECT '05: Role not found';
RETURN
END

BEGIN
INSERT @users_in_role(role_id, [user_id])
SELECT role_id, id
FROM tbl_admin_users(nolock) WHERE role_id = @role_id;
SET @users_in_role_count = (SELECT COUNT(1) FROM @users_in_role);

IF (@users_in_role_count > 0)
BEGIN
    SELECT '04: Role is currently assigned to at least 1 user and cannot be deleted';
RETURN
END
END

    BEGIN TRY
        BEGIN TRAN

DELETE FROM tbl_admin_role_permissions where role_id = @role_id;

DELETE FROM tbl_admin_roles where id = @role_id;

SELECT '00: Success';

        COMMIT TRAN
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        RETURN ERROR_MESSAGE()
    END CATCH
END
GO