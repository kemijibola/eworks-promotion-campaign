PRINT 'Inserting system permissions'
--insert system permissions
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanCreateAdminRole', N'Ability to create admin role');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewAdminRole', N'Ability to view admin role');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewAdminRoles', N'Ability to view admin roles');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanCreateAdminUser', N'Ability to create admin user');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanUpdateAdminRoleStatus', N'Ability to update admin role status');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanUpdateAdminUserStatus', N'Ability to update admin user status');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanUpdateAdminPermissionStatus', N'Ability to update admin permission status');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanAssignPermissionsToRole', N'Ability to assign permissions to role');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewUsers', N'Ability to view users');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewPermissions', N'Ability to view permissions');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewPermission', N'Ability to view permission');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewUser', N'Ability to view user');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewAdminUser', N'Ability to view admin user');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewAdminUsers', N'Ability to view admin users');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanEditAdminUser', N'Ability to edit admin user');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanCreateCampaign', N'Ability to create campaign');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanConfigureCampaign', N'Ability to create campaign configurations');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanCreateSystemConfiguration', N'Ability to create system configurations');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanEditCampaign', N'Ability to edit campaign');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanStartCampaign', N'Ability to start campaign');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanPauseCampaign', N'Ability to pause campaign');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanCreateCampaignReward', N'Ability to create campaign reward');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanCreateRaffleReward', N'Ability to create raffle reward');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanDeleteCampaignReward', N'Ability to delete campaign reward');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanDeleteRaffleReward', N'Ability to delete raffle reward');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanDeActivateUser', N'Ability to deactivate user');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanBlacklistUser', N'Ability to blacklist user');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanBlacklistUsers', N'Ability to upload blacklist file');


DECLARE @adminId INT
DECLARE @roleId INT
DECLARE @PermissionId int

PRINT 'Inserting Super Admin role'
--create Super Admin role
INSERT INTO [dbo].[tbl_admin_roles] ([role_name], [role_description]) VALUES ( N'Super Admin', N'Super Admin')
SET @roleId = SCOPE_IDENTITY();

PRINT 'Inserting Super Admin user'
--create super admin user
INSERT INTO [dbo].[tbl_admin_users] ([first_name], [last_name], [email], [phone], [role_id], [password_hash], [password_salt]) VALUES (N'Udochukwu', N'Anumudu', N'kabnallymoney@icloud.com', N'07082987128', @roleId, N'B/xE3M4fot5Wh1mqaXDG1/fZ1RLtNa9eIYg6Tp4TVX4=', N'oh8cjiO9JlGZB7BiVeZ1kQ==')
SET @adminId = SCOPE_IDENTITY();

--approve super admin role
PRINT 'Update Super Admin role approval status'
UPDATE [dbo].[tbl_admin_roles] SET [status]=1,[status_updated_by]=@adminId, [status_comment]=N'Approved' ,[status_updated_at]=getdate() WHERE Id=@roleId;

--approve super admin user
PRINT 'Update Super Admin user approval status'
UPDATE [dbo].[tbl_admin_users] SET [status]=1,[status_updated_by]=@adminId, [status_comment]=N'Approved', [status_updated_at]=getdate() WHERE Id=@adminId;

PRINT 'Approving system permissions'
--approve system permissions
DECLARE MY_CURSOR CURSOR
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR
SELECT DISTINCT id
FROM tbl_admin_permissions;

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @PermissionId
PRINT 'Updating permissions'
WHILE @@FETCH_STATUS = 0
BEGIN
UPDATE [dbo].[tbl_admin_permissions] SET [status]=1,[status_updated_by]=@adminId, [status_comment]=N'Approved', [status_updated_at]=getdate() WHERE Id=@PermissionId;
FETCH NEXT FROM MY_CURSOR INTO @PermissionId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

PRINT 'Inserting Super Admin permissions'
--insert default permissions for Super Admin role
DECLARE MY_CURSOR CURSOR
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR
SELECT DISTINCT id
FROM tbl_admin_permissions;

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @PermissionId
PRINT 'Adding permissions to Super Admin Role'
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO [dbo].[tbl_admin_role_permissions] ([role_id], [permission_id]) VALUES (@roleId, @PermissionId)
FETCH NEXT FROM MY_CURSOR INTO @PermissionId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR