USE [library_db]
GO

/****** Object:  Table [dbo].[student]    Script Date: 06/17/2019 15:56:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[student](
	[id] [varchar](30) NOT NULL,
	[name] [nchar](100) NOT NULL,
	[bod] [nchar](100) NOT NULL,
	[class_id] [varchar](10) NOT NULL,
	[email] [varchar](200) NULL,
	[address] [text] NULL,
 CONSTRAINT [PK_student] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[student]  WITH CHECK ADD  CONSTRAINT [class_fk] FOREIGN KEY([class_id])
REFERENCES [dbo].[class] ([id])
GO

ALTER TABLE [dbo].[student] CHECK CONSTRAINT [class_fk]
GO

