USE [library_db]
GO

/****** Object:  Table [dbo].[book_record]    Script Date: 06/17/2019 15:56:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[book_record](
	[id] [varchar](10) NOT NULL,
	[book_code] [varchar](100) NOT NULL,
	[book_id] [varchar](10) NOT NULL,
	[additional_info] [text] NULL,
	[available] [smallint] NULL,
 CONSTRAINT [PK_book_record] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[book_record]  WITH CHECK ADD  CONSTRAINT [FK_book_record_book] FOREIGN KEY([book_id])
REFERENCES [dbo].[book] ([id])
GO

ALTER TABLE [dbo].[book_record] CHECK CONSTRAINT [FK_book_record_book]
GO

