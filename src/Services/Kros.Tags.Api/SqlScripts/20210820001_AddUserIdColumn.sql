﻿SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Tags]
ADD [UserId] [int] NOT NULL;

CREATE INDEX [IX_Tags_UserId] ON [dbo].[Tags] ([UserId]);