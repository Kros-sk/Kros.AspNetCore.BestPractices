SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

INSERT INTO [dbo].[ToDos] ([Id], [Name], [Description], [UserId], [Created]) VALUES (1, 'CQRS', 'Study the CQRS design pattern', 1, '20190401  11:25:11 AM');
INSERT INTO [dbo].[ToDos] ([Id], [Name], [Description], [UserId], [Created]) VALUES (2, 'MediatR', 'Study the MediatR project', 1, '20190402  10:15:09 AM');
INSERT INTO [dbo].[ToDos] ([Id], [Name], [Description], [UserId], [Created]) VALUES (3, 'Create prototype', 'Create prototype with MediatR', 1, '20190402  10:40:00 AM');
INSERT INTO [dbo].[ToDos] ([Id], [Name], [Description], [UserId], [Created]) VALUES (4, 'ToDo', 'ToDo for user 2', 2, '20190402  10:41:00 AM');

INSERT INTO [dbo].[IdStore] VALUES ('ToDos', 4)