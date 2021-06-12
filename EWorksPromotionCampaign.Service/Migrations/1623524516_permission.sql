IF (OBJECT_ID('usp_fetch_permission_by_id') is not null)
    BEGIN
        drop procedure usp_fetch_permission_by_id;
    END
GO

create procedure usp_fetch_permission_by_id 
@id bigint
as
    SELECT *
    FROM tbl_admin_permissions(nolock)
    WHERE Id = @id;
GO

IF (OBJECT_ID('usp_fetch_role_by_role_name') is not null)
    BEGIN
        drop procedure usp_fetch_role_by_role_name;
    END
GO

create procedure usp_fetch_role_by_role_name 
@role_name varchar(50)
as
    SELECT *
    FROM tbl_admin_roles(nolock)
    WHERE role_name = @role_name;
GO