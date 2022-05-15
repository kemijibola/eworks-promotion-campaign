PRINT 'Inserting system permissions'
--insert system permissions
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewCampaign', N'Ability to view campaign');
INSERT INTO [dbo].[tbl_admin_permissions] ( [permission_name], [permission_description]) VALUES ( N'CanViewCampaigns', N'Ability to view campaigns');