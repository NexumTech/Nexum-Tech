USE [NexumTech]
GO

/****** Object:  Table [dbo].[tb_employee]    Script Date: 03/06/2024 22:44:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tb_employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[CompanyId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tb_employee]  WITH CHECK ADD  CONSTRAINT [FK__tb_employ__Compa__5070F446] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[tb_company] ([Id])
GO

ALTER TABLE [dbo].[tb_employee] CHECK CONSTRAINT [FK__tb_employ__Compa__5070F446]
GO

ALTER TABLE [dbo].[tb_employee]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[tb_user] ([Id])
GO


