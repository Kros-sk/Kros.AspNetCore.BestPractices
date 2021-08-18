SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Tags]
ADD [OrganizationId] [int] NOT NULL;

CREATE INDEX [IX_Tags_OrganizationId] ON [dbo].[Tags] ([OrganizationId]);
GO

ALTER TABLE [dbo].[Tags]
ADD [Description] [nvarchar](40) NULL;

GO

