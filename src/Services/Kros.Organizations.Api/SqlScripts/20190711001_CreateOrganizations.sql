SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Organizations](
	[Id] bigint IDENTITY(1,1) NOT NULL,
	[UserId] bigint NOT NULL,
	[OrganizationName] nvarchar(50),
	[BusinessId] nvarchar(15),
	[Street] nvarchar(30),
	[StreetNumber] nvarchar(20),
	[City] nvarchar(35),
	[ZipCode] nvarchar(10)
 CONSTRAINT [PK_Organizations] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO