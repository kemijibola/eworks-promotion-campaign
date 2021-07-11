IF (OBJECT_ID('usp_delete_campaign_reward') is not null)
    BEGIN
        drop procedure usp_delete_campaign_reward;
    END
GO

create procedure usp_delete_campaign_reward  
    @id bigint
as
    DELETE FROM tbl_campaign_rewards
    WHERE id =@id;
GO