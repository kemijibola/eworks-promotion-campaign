IF (OBJECT_ID('usp_create_new_user') is not null)
BEGIN
	drop procedure usp_create_new_user;
END
GO

create procedure usp_create_new_user
	@first_name varchar(50),
	@middle_name varchar(50) = null,
	@last_name varchar(50),
	@phone varchar(50),
	@email varchar(50),
	@date_of_birth datetime,
	@address varchar(100),
	@password_hash nvarchar(100),
	@password_salt nvarchar(100)
as
	INSERT into tbl_users(first_name, middle_name, last_name, phone, email, date_of_birth, [address], password_hash, password_salt) VALUES
	(@first_name, @middle_name, @last_name, @phone, @email, @date_of_birth, @address, @password_hash, @password_salt) SELECT SCOPE_IDENTITY();
GO

IF (OBJECT_ID('usp_get_user_by_email_phone') is not null)
BEGIN
	drop procedure usp_get_user_by_email_phone;
END
go

create procedure usp_get_user_by_email_phone
	@email varchar (50),
	@phone varchar(50)
as
SELECT Top 1 * FROM tbl_users where email = @email OR  phone = @phone;
GO

IF (OBJECT_ID('usp_get_user_by_identifier') is not null)
    BEGIN
        drop procedure usp_get_user_by_identifier;
    END
GO

create procedure usp_get_user_by_identifier
@identifier varchar (50)
as
SELECT Top 1 * FROM tbl_users where email = @identifier OR phone = @identifier;
GO

IF (OBJECT_ID('usp_update_user_password') is not null)
    BEGIN
        drop procedure usp_update_user_password;
    END
go

create procedure usp_update_user_password @email varchar(50),
                                          @password_hash nvarchar(100),
                                          @password_salt nvarchar(100)
as

UPDATE tbl_users
SET password_hash = @password_hash,
    password_salt =@password_salt
where email = @email
GO

IF (OBJECT_ID('usp_insert_token_request') is not null)
    BEGIN
        drop procedure usp_insert_token_request;
    END
GO

create procedure usp_insert_token_request       @requester varchar(50),
                                                @token varchar(100),
                                                @salt nvarchar(100) = null,
                                                @type_of_token varchar(10),
                                                @request_id varchar(50)

as
INSERT into tbl_token_requests(requester, token, salt, type_of_token, request_id)
VALUES (@requester, @token, @salt, @type_of_token, @request_id);
GO

IF (OBJECT_ID('usp_delete_token_request_by_request_id') is not null)
    BEGIN
        drop procedure usp_delete_token_request_by_request_id;
    END
GO

create procedure usp_delete_token_request_by_request_id @request_id varchar(50)
as
    DELETE
    FROM tbl_token_requests
    where request_id = @request_id;
GO

IF (OBJECT_ID('usp_get_token_request_by_request_id') is not null)
BEGIN
	drop procedure usp_get_token_request_by_request_id;
END
GO

create procedure usp_get_token_request_by_request_id @request_id varchar(50)
as
    SELECT Top 1 *
    FROM tbl_token_requests
    where request_id = @request_id;
GO

IF (OBJECT_ID('usp_get_token_request_by_token') is not null)
BEGIN
	drop procedure usp_get_token_request_by_token;
END
GO

create procedure usp_get_token_request_by_token @token varchar(100)
as
    SELECT Top 1 *
    FROM tbl_token_requests
    where token = @token;
GO