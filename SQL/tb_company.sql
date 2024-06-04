USE [NexumTech]
GO

/****** Object:  Table [dbo].[tb_company]    Script Date: 03/06/2024 22:43:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tb_company](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OwnerId] [int] NOT NULL,
	[Name] [varchar](80) NOT NULL,
	[Description] [varchar](400) NOT NULL,
	[Logo] [varbinary](max) NOT NULL,
 CONSTRAINT [PK__tb_compa__3214EC07EB5E89C2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tb_company]  WITH CHECK ADD  CONSTRAINT [FK__tb_compan__Owner__4D94879B] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[tb_user] ([Id])
GO

ALTER TABLE [dbo].[tb_company] CHECK CONSTRAINT [FK__tb_compan__Owner__4D94879B]
GO

ALTER TABLE [dbo].[tb_company]  WITH CHECK ADD  CONSTRAINT [FK_tb_companOwner_02FC7413] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[tb_user] ([Id])
GO

ALTER TABLE [dbo].[tb_company] CHECK CONSTRAINT [FK_tb_companOwner_02FC7413]
GO


