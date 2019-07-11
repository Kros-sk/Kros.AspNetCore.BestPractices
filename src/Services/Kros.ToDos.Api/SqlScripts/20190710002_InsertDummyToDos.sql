SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

INSERT INTO [dbo].[ToDos] ([Id], [Name], [Description], [UserId], [OrganizationId], [Created], [LastChange], [IsDone]) VALUES (1, 'CQRS', 'Study the CQRS design pattern', 1, 1, '20190701  11:25:11 AM', '20190701  11:25:11 AM', 0);
INSERT INTO [dbo].[ToDos] ([Id], [Name], [Description], [UserId], [OrganizationId], [Created], [LastChange], [IsDone]) VALUES (2, 'MediatR', 'Study the MediatR project', 1, 1, '20190702  10:15:09 AM', '20190702  10:15:09 AM', 0);
INSERT INTO [dbo].[ToDos] ([Id], [Name], [Description], [UserId], [OrganizationId], [Created], [LastChange], [IsDone]) VALUES (3, 'Create prototype', 'Create prototype with MediatR', 1, 1, '20190702  10:40:00 AM', '20190702  10:40:00 AM', 0);
INSERT INTO [dbo].[ToDos] ([Id], [Name], [Description], [UserId], [OrganizationId], [Created], [LastChange], [IsDone]) VALUES (4, 'ToDo', 'ToDo for user 2', 2, 2, '20190702  10:41:00 AM', '20190702  10:41:00 AM', 0);

GO

DELETE FROM [dbo].[IdStore] WHERE [TableName] = 'ToDos';
INSERT INTO [dbo].[IdStore] VALUES ('ToDos', 4);

GO