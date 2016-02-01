USE [Ears]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 18/01/2016 21:18:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GcmUserId] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Location] [geography] NULL,
	[LastSeenOn] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO


CREATE TABLE [dbo].[Crew](
	[Id] UniqueIdentifier NOT NULL,
	[ApplicationId] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Location] [geography] NULL,
	[LastSeenOn] [datetime] NULL,
 CONSTRAINT [PK_Crew] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
CREATE TABLE [dbo].[Callout](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Crew] UniqueIdentifier NOT NULL,
	[Route] [nvarchar](max) NOT NULL,
	[Location] [geography] NOT NULL,
	[LastSignal] [datetime] NOT NULL,
	[IsFinished] [bit] NOT NULL,
	[LastBroadcast] [datetime] NULL,
 CONSTRAINT [PK_Callout] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Callout]  WITH CHECK ADD  CONSTRAINT [FK_Callout_Crew] FOREIGN KEY([Crew])
REFERENCES [dbo].[Crew] ([Id])
GO

ALTER TABLE [dbo].[Callout] CHECK CONSTRAINT [FK_Callout_Crew]
GO

CREATE  PROCEDURE GetNextBatchOfUsersToBeNotified
AS
begin
declare @id int, @location geography
SELECT TOP 1 @id = [Id]
      ,@location = [Location] 
  FROM [Callout]
  where IsFinished = 0 --and LastSignal > DATEADD(s,-3000, GETDATE())
  ORDER BY (CASE WHEN [LastBroadcast] IS NULL THEN 0 ELSE 1 END),
         [LastBroadcast] DESC
 
 select top 1000 * from [Users] 
 where IsActive = 0 and @location.STDistance(Location)< 500000 
 
 update [Callout] set LastBroadcast = GETDATE() where @id =id
 
 end
