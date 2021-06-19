IF OBJECT_ID('tbl_configurations', 'U') IS NULL
    begin
        create TABLE [dbo].[tbl_configurations]
        (
            [id]                   bigint             NOT NULL IDENTITY (1, 1) PRIMARY KEY,
            [key]                  varchar(100)       NOT NULL,
            [value]                nvarchar(1024)      NOT NULL,
        ) ON [PRIMARY]
    end
go

IF (OBJECT_ID('usp_create_new_configuration') is not null)
BEGIN
	drop procedure usp_create_new_configuration;
END
GO

create procedure usp_create_new_configuration
	@key varchar(100),
	@value nvarchar(1024)
as
	INSERT into tbl_configurations([key], [value]) VALUES
	(@key, @value) SELECT SCOPE_IDENTITY();
GO

IF (OBJECT_ID('usp_get_configuration_by_type') is not null)
BEGIN
	drop procedure usp_get_configuration_by_type;
END
go

create procedure usp_get_configuration_by_type
	@key varchar (100)
as
SELECT Top 1 * FROM tbl_configurations(nolock) where [key] = @key;
GO


IF (OBJECT_ID('usp_delete_configuration') is not null)
    BEGIN
        drop procedure usp_delete_configuration;
    END
GO

create procedure usp_delete_configuration  @config_id bigint
as
    DELETE FROM tbl_configurations
    WHERE id = @config_id
GO

IF (OBJECT_ID('usp_update_configuration') is not null)
    BEGIN
        drop procedure usp_update_configuration;
    END
GO

create procedure usp_update_configuration  @config_id bigint,
                                  @value varchar(1024)
as
UPDATE tbl_configurations
SET [value] = @value
WHERE id = @config_id
GO

IF (OBJECT_ID('usp_get_configurations') is not null)
    BEGIN
        drop procedure usp_get_configurations;
    END
GO

create procedure usp_get_configurations
as
    SELECT * FROM tbl_configurations(nolock);
GO