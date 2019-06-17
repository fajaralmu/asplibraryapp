USE [library_db]
GO

/****** Object:  Table [dbo].[issue]    Script Date: 06/17/2019 15:56:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[issue](
	[id] [varchar](10) NOT NULL,
	[date] [datetime] NOT NULL,
	[user_id] [varchar](10) NOT NULL,
	[student_id] [varchar](30) NOT NULL,
	[addtional_info] [text] NULL,
	[type] [nchar](10) NOT NULL,
 CONSTRAINT [PK_issue] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[issue]  WITH CHECK ADD  CONSTRAINT [FK_student_issue] FOREIGN KEY([student_id])
REFERENCES [dbo].[student] ([id])
GO

ALTER TABLE [dbo].[issue] CHECK CONSTRAINT [FK_student_issue]
GO

ALTER TABLE [dbo].[issue]  WITH CHECK ADD  CONSTRAINT [FK_user_issue] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([id])
GO

ALTER TABLE [dbo].[issue] CHECK CONSTRAINT [FK_user_issue]
GO

