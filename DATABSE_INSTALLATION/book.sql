USE [library_db]
GO

/****** Object:  Table [dbo].[book]    Script Date: 06/17/2019 15:55:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[book](
	[id] [varchar](10) NOT NULL,
	[title] [varchar](150) NOT NULL,
	[author_id] [varchar](10) NOT NULL,
	[category_id] [varchar](10) NOT NULL,
	[publisher_id] [varchar](10) NOT NULL,
	[isbn] [varchar](100) NULL,
	[review] [text] NULL,
	[page] [int] NULL,
	[img] [text] NULL,
 CONSTRAINT [PK_book] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[book]  WITH CHECK ADD  CONSTRAINT [author_fk] FOREIGN KEY([author_id])
REFERENCES [dbo].[author] ([id])
GO

ALTER TABLE [dbo].[book] CHECK CONSTRAINT [author_fk]
GO

ALTER TABLE [dbo].[book]  WITH CHECK ADD  CONSTRAINT [category_fk] FOREIGN KEY([category_id])
REFERENCES [dbo].[category] ([id])
GO

ALTER TABLE [dbo].[book] CHECK CONSTRAINT [category_fk]
GO

ALTER TABLE [dbo].[book]  WITH CHECK ADD  CONSTRAINT [publisher_fk] FOREIGN KEY([publisher_id])
REFERENCES [dbo].[publisher] ([id])
GO

ALTER TABLE [dbo].[book] CHECK CONSTRAINT [publisher_fk]
GO

