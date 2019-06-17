USE [library_db]
GO

/****** Object:  Table [dbo].[book_issue]    Script Date: 06/17/2019 15:56:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[book_issue](
	[id] [varchar](10) NOT NULL,
	[book_record_id] [varchar](10) NOT NULL,
	[issue_id] [varchar](10) NOT NULL,
	[qty] [int] NOT NULL,
	[ref_issue] [nchar](10) NOT NULL,
	[book_return] [smallint] NULL,
	[book_issue_id] [varchar](10) NULL,
 CONSTRAINT [PK_book_issue] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[book_issue]  WITH CHECK ADD  CONSTRAINT [FK_book_issue_book_issue] FOREIGN KEY([book_issue_id])
REFERENCES [dbo].[book_issue] ([id])
GO

ALTER TABLE [dbo].[book_issue] CHECK CONSTRAINT [FK_book_issue_book_issue]
GO

ALTER TABLE [dbo].[book_issue]  WITH CHECK ADD  CONSTRAINT [FK_book_issue_book_record] FOREIGN KEY([book_record_id])
REFERENCES [dbo].[book_record] ([id])
GO

ALTER TABLE [dbo].[book_issue] CHECK CONSTRAINT [FK_book_issue_book_record]
GO

ALTER TABLE [dbo].[book_issue]  WITH CHECK ADD  CONSTRAINT [issue_fk] FOREIGN KEY([issue_id])
REFERENCES [dbo].[issue] ([id])
GO

ALTER TABLE [dbo].[book_issue] CHECK CONSTRAINT [issue_fk]
GO

