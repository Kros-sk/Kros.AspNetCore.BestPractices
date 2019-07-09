SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserOrganization](
	[UserId] [int] NOT NULL,
	[OrganizationId] [int] NOT NULL
 CONSTRAINT [PK_UserOrganization] PRIMARY KEY CLUSTERED
(
	[UserId],
	[OrganizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Index [NCI_UserId]    Script Date: 18.5.2018 7:00:56 ******/
CREATE NONCLUSTERED INDEX [NCI_UserId] ON [dbo].[UserOrganization]
(
	[UserId] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [NCI_CompanyId]    Script Date: 18.5.2018 7:00:56 ******/
CREATE NONCLUSTERED INDEX [NCI_OrganizationId] ON [dbo].[UserOrganization]
(
	[OrganizationId] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/* Foreign keys */
ALTER TABLE [dbo].[UserOrganization]  WITH CHECK ADD  CONSTRAINT [FK_UserOrganization_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([Id])
GO
ALTER TABLE [dbo].[UserOrganization] CHECK CONSTRAINT [FK_UserOrganization_Organizations]
GO