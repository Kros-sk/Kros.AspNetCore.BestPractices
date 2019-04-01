﻿SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ToDos](
	[Id] [int] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Name] [nvarchar](50) NULL,
	[Created] [datetime2](7) NULL,
 CONSTRAINT [PK_ToDos] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO