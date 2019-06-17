USE [library_db]
GO

/****** Object:  Table [dbo].[author]    Script Date: 06/17/2019 15:55:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[author](
	[id] [varchar](10) NOT NULL,
	[name] [nchar](100) NOT NULL,
	[address] [nchar](100) NULL,
	[email] [nchar](200) NULL,
	[phone] [nchar](200) NULL,
 CONSTRAINT [PK_author] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

