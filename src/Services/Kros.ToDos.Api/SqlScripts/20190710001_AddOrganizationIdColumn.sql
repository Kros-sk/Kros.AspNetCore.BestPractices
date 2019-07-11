SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DELETE FROM [dbo].[ToDos];
GO

DELETE FROM [dbo].[IdStore] WHERE [TableName] = 'ToDos';
GO

ALTER TABLE [dbo].[ToDos]
ADD [OrganizationId] [int] NOT NULL;

CREATE INDEX [IX_ToDos_OrganizationId] ON [dbo].[ToDos] ([OrganizationId]);
GO