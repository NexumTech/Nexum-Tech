USE [NexumTech]
GO

/****** Object:  Table [dbo].[tb_user]    Script Date: 03/06/2024 22:45:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tb_user](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](80) NOT NULL,
	[Email] [varchar](80) NOT NULL,
	[Photo] [varbinary](max) NULL,
	[Password] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


