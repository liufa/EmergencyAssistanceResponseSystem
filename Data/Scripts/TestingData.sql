declare @i int= 0,@lat int =0, @long int= 0

while (@i < 10000)
begin
 set @lat =(select (0.9 -Rand()*1.8)*100)
 set @long =(select (0.9 -Rand()*1.8)*100)
insert into [dbo].[Users] ([GcmUserId]
      ,[CreatedOn]
      ,[Location]
      ,[LastSeenOn])
select CONVERT(varchar(10),@i+10000),GETDATE(),geography::Point(@lat, @long,4326),GETDATE()
set @i =@i+1
 
end
go
/****** Script for SelectTopNRows command from SSMS  ******/
declare @i int= 0, @lat int =0, @long int= 0

while (@i < 100)
begin
 set @lat =(select (0.9 -Rand()*1.8)*100)
 set @long =(select (0.9 -Rand()*1.8)*100)
 
insert into [Crew](
      [GoogleUserId]
      ,[CreatedOn]
      ,[Location]
      ,[LastSeenOn])
 select CONVERT(varchar(10), @i+10000),GETDATE(), geography::Point(@lat, @long,4326), GETDATE()

insert into [Callout] ([Crew]
      ,[Route]
      ,[Location]
      ,[LastSignal]
      ,[IsFinished])
select IDENT_CURRENT('Crew'),CONVERT(varchar(10),@i+10000),geography::Point(@lat, @long,4326),GETDATE(), 0
set @i =@i+1
 
end