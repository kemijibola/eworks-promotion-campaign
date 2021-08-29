IF (OBJECT_ID('usp_get_admin_users_overview') is not null)
    BEGIN
        drop procedure usp_get_admin_users_overview;
    END
GO

create procedure usp_get_admin_users_overview

@page_number int = 1,
@page_size int,
@search_text varchar(50) = null

as
declare @from_row int = 1;

if @page_number > 1
begin
set @from_row = ((@page_number * @page_size) - (@page_size)) + 1;
end;

with records as
(
    select tblu.id, tblu.first_name+ ' ' +tblu.last_name [name], tblu.email, tblr.[role_name] role_name, tblu.[status], tblu.created_at,
    row_number()  over(order by tblu.created_at desc) as row_num
    from tbl_admin_users(nolock) tblu
    inner join tbl_admin_roles(nolock) tblr on tblu.role_id = tblr.id

    where ((@search_text is null)
    or (tblu.first_name + ' ' + tblu.last_name like '%'+ @search_text +'%')
    or (tblu.email like '%'+ @search_text +'%'))),
    rec_count as
    (
    select count(*) total_count from records
    )
select * from records,rec_count where row_num between @from_row and (@from_row + @page_size -1) order by created_at desc
GO