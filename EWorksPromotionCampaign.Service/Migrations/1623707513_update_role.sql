IF (OBJECT_ID('usp_update_role') is not null)
    BEGIN
        drop procedure usp_update_role;
    END
GO

create procedure usp_update_role  @role_name varchar(50),
                                         @role_description varchar(50),
                                         @role_id bigint
as
UPDATE tbl_admin_roles
SET role_name = @role_name,
    role_description  = @role_description,
	updated_at = getdate()
WHERE id = @role_id
GO